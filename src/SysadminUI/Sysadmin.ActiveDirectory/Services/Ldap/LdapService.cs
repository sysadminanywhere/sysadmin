
using Sysadmin.ActiveDirectory.Services.Ldap;
using System.DirectoryServices.Protocols;
using System.Net;

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

        private SearchService searchService;

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
            this.server = server;
            this.credential = credential;

            LdapDirectoryIdentifier ldapDirectoryIdentifier = null;
            NetworkCredential networkCredential = null;
            AuthType authType = AuthType.Basic;

            try
            {
                if (server != null && !string.IsNullOrEmpty(server.ServerName))
                {
                    ldapDirectoryIdentifier = new LdapDirectoryIdentifier(server.ServerName, server.Port);
                }

                if (server != null && server.IsSSL)
                {
                    authType = AuthType.Msn;
                }

                if (credential != null && !string.IsNullOrEmpty(credential.UserName) && !string.IsNullOrEmpty(credential.Password))
                {
                    networkCredential = new NetworkCredential(credential.UserName, credential.Password);
                }

                ldapConnection = new LdapConnection(ldapDirectoryIdentifier, networkCredential, authType);
                searchService = new SearchService(ldapConnection);

                var searchEntries = searchService.Search("", "(objectclass=*)", SearchScope.OneLevel, null);
                DefaultNamingContext = searchEntries[0].Attributes["defaultNamingContext"].ToString();
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

        public async Task<SearchResultEntryCollection> SearchAsync(string filter)
        {
            if (ldapConnection == null)
                throw new ArgumentNullException(nameof(ldapConnection));

            if (string.IsNullOrEmpty(filter))
                throw new ArgumentNullException(nameof(filter));

            return searchService.Search(DefaultNamingContext, filter);
        }

        public async Task<SearchResultEntryCollection> SearchAsync(string path, string filter)
        {
            if (ldapConnection == null)
                throw new ArgumentNullException(nameof(ldapConnection));

            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            if (string.IsNullOrEmpty(filter))
                throw new ArgumentNullException(nameof(filter));

            var entries = await ldapConnection.SearchAsync(path, filter);

            return entries.ToList();
        }

        public async Task<SearchResultEntryCollection> SearchAsync(string path, string filter, LdapSearchScope scope = LdapSearchScope.LDAP_SCOPE_SUBTREE)
        {
            if (ldapConnection == null)
                throw new ArgumentNullException(nameof(ldapConnection));

            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            if (string.IsNullOrEmpty(filter))
                throw new ArgumentNullException(nameof(filter));

            var entries = await ldapConnection.SearchAsync(path, filter, scope: scope);

            return entries.ToList();
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

        public void Dispose()
        {
            ldapConnection?.Dispose();
        }

        public async Task<DirectoryResponse?> SendRequestAsync(DirectoryRequest directoryRequest)
        {
            if (ldapConnection == null)
                throw new ArgumentNullException(nameof(ldapConnection));

            if (directoryRequest == null)
                throw new ArgumentNullException(nameof(directoryRequest));

            if (ldapConnection != null)
            {
                return await ldapConnection.SendRequestAsync(directoryRequest);
            }
            return null;
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

    }
}