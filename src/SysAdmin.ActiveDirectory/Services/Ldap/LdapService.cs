﻿
using LdapForNet;
using System.Collections.Generic;
using static LdapForNet.Native.Native;

namespace SysAdmin.ActiveDirectory.Services.Ldap
{
    public class LdapService : IDisposable
    {

        LdapConnection? ldapConnection = null;

        public string DefaultNamingContext { get; private set; } = string.Empty;
        public string DomainName { get; private set; } = string.Empty;

        IServer? server;
        ICredential? credential;

        public ICredential? Credential { get { return credential; } }
        public IServer? Server { get { return server; } }

        public LdapService(IServer server)
        {
            ConnectAsync(server, null).Wait();
        }

        public LdapService(ICredential credential)
        {
            ConnectAsync(null, credential).Wait();
        }

        public LdapService(IServer server, ICredential credential)
        {
            ConnectAsync(server, credential).Wait();
        }

        private async Task ConnectAsync(IServer? server, ICredential? credential)
        {

            /*
             You can install and configure the Active Directory Certificate Services (AD CS) role on a domain controller.
             */

            this.server = server;
            this.credential = credential;

            try
            {
                ldapConnection = new LdapConnection();

                if (server != null && !string.IsNullOrEmpty(server.ServerName))
                {
                    ldapConnection.Connect(server.ServerName, server.Port, server.IsSSL ? LdapSchema.LDAPS : LdapSchema.LDAP);

                    if (server.IsSSL)
                        ldapConnection.StartTransportLayerSecurity(true);
                }
                else
                {
                    ldapConnection.Connect();
                }

                if (server != null && server.IsSSL)
                {
                    ldapConnection.TrustAllCertificates();
                }

                if (credential != null && !string.IsNullOrEmpty(credential.UserName) && !string.IsNullOrEmpty(credential.Password))
                {
                    await ldapConnection.BindAsync(LdapAuthType.Negotiate, new LdapCredential()
                    {
                        UserName = credential.UserName,
                        Password = credential.Password
                    });
                }
                else
                {
                    await ldapConnection.BindAsync();
                }

                var searchEntries = await ldapConnection.SearchAsync(null, "(objectclass=*)", scope: LdapSearchScope.LDAP_SCOPE_BASE);
                DefaultNamingContext = searchEntries[0].DirectoryAttributes["defaultNamingContext"].GetValue<string>();
                DomainName = DefaultNamingContext.ToUpper().Replace("DC=", "").Replace(",", ".").ToLower();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                ldapConnection = null;
            }
        }

        public bool IsConnected => ldapConnection != null;

        public string ErrorMessage { get; private set; } = string.Empty;

        public async Task<List<LdapEntry>> SearchAsync(string filter)
        {
            if (ldapConnection == null)
                throw new ArgumentNullException(nameof(ldapConnection));

            if (string.IsNullOrEmpty(filter))
                throw new ArgumentNullException(nameof(filter));

            return await SearchAsync(DefaultNamingContext, filter);
        }

        public async Task<List<LdapEntry>> SearchAsync(string path, string filter, LdapSearchScope scope = LdapSearchScope.LDAP_SCOPE_SUBTREE)
        {
            if (ldapConnection == null)
                throw new ArgumentNullException(nameof(ldapConnection));

            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            if (string.IsNullOrEmpty(filter))
                throw new ArgumentNullException(nameof(filter));

            //var entries = await ldapConnection.SearchAsync(path, filter, scope: scope);
            //return entries.ToList();

            var directoryRequest = new SearchRequest(path, filter, scope);
            var pageSize = 100;

            var vlvRequestControl = new VlvRequestControl(0, pageSize - 1, 1);
            directoryRequest.Controls.Add(new SortRequestControl("cn", false));
            directoryRequest.Controls.Add(vlvRequestControl);

            List<LdapEntry> results = new List<LdapEntry>();

            while (true)
            {
                var response = (SearchResponse)ldapConnection.SendRequest(directoryRequest);
                results.AddRange(response.Entries.Select(_ => _.ToLdapEntry()).ToList());

                var vlvResponseControl = (VlvResponseControl)response.Controls.Single(_ => _.GetType() == typeof(VlvResponseControl));
                vlvRequestControl.Offset += pageSize;
                if (vlvRequestControl.Offset > vlvResponseControl.ContentCount)
                {
                    break;
                }
            }

            return results;
        }

        public async Task<ModifyResponse> SendRequestAsync(ModifyRequest modifyRequest)
        {
            if (ldapConnection == null)
                throw new ArgumentNullException(nameof(ldapConnection));

            if (modifyRequest == null)
                throw new ArgumentNullException(nameof(modifyRequest));

            return (ModifyResponse)await ldapConnection.SendRequestAsync(modifyRequest);
        }

        public async Task AddAsync(LdapEntry entry)
        {
            if (ldapConnection == null)
                throw new ArgumentNullException(nameof(ldapConnection));

            if (entry == null)
                throw new ArgumentNullException(nameof(entry));

            for (int i = entry.DirectoryAttributes.Count - 1; i >= 0; i--)
            {
                if (string.IsNullOrEmpty(entry.DirectoryAttributes[i].GetValue<string>()))
                {
                    entry.DirectoryAttributes.RemoveAt(i);
                }
            }

            if (ldapConnection != null)
            {
                await ldapConnection.AddAsync(entry);
            }
        }

        public async Task ModifyAsync(LdapModifyEntry entry)
        {
            if (ldapConnection == null)
                throw new ArgumentNullException(nameof(ldapConnection));

            if (entry == null)
                throw new ArgumentNullException(nameof(entry));

            if (ldapConnection != null)
            {
                await ldapConnection.ModifyAsync(entry);
            }
        }

        public async Task ModifyPropertyAsync(string dn, string name, string value)
        {
            if (ldapConnection == null)
                throw new ArgumentNullException(nameof(ldapConnection));

            if (string.IsNullOrEmpty(dn))
                throw new ArgumentNullException(nameof(dn));

            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(value));

            var attribute = new DirectoryModificationAttribute()
            {
                Name = name,
                LdapModOperation = LdapModOperation.LDAP_MOD_REPLACE
            };

            attribute.Add(value);

            await ldapConnection.SendRequestAsync(new ModifyRequest(dn, attribute));
        }

        public async Task DeleteAsync(string dn)
        {
            if (ldapConnection == null)
                throw new ArgumentNullException(nameof(ldapConnection));

            if (string.IsNullOrEmpty(dn))
                throw new ArgumentNullException(nameof(dn));

            await ldapConnection.DeleteAsync(dn);
        }

        public async Task<DirectoryResponse?> SendRequestAsync(DirectoryRequest directoryRequest)
        {
            if (ldapConnection == null)
                throw new ArgumentNullException(nameof(ldapConnection));

            if (directoryRequest == null)
                throw new ArgumentNullException(nameof(directoryRequest));

            return await ldapConnection.SendRequestAsync(directoryRequest);
        }

        public async Task<List<string>> WellKnownObjectsAsync()
        {
            if (ldapConnection == null)
                throw new ArgumentNullException(nameof(ldapConnection));

            var searchEntries = await ldapConnection.SearchAsync(DefaultNamingContext, "(objectclass=domain)", scope: LdapSearchScope.LDAP_SCOPE_BASE);
            return searchEntries[0].DirectoryAttributes["wellKnownObjects"].GetValues<string>().ToList();
        }
        public LdapEntry GetRootDse()
        {
            if (ldapConnection == null)
                throw new ArgumentNullException(nameof(ldapConnection));

            return ldapConnection.GetRootDse();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            ldapConnection?.Dispose();
        }

    }
}