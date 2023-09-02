using LdapForNet;
using SysAdmin.ActiveDirectory.Models;
using SysAdmin.ActiveDirectory.Services.Ldap;
using System.DirectoryServices.AccountManagement;
using System.Runtime.InteropServices;
using System.Text;
using static LdapForNet.Native.Native;

namespace SysAdmin.ActiveDirectory.Repositories
{
    public class UsersRepository : IDisposable
    {

        private LdapService ldapService;

        public UsersRepository(LdapService ldapService)
        {
            if (ldapService == null)
                throw new ArgumentNullException(nameof(ldapService));

            this.ldapService = ldapService;
        }

        public async Task<List<UserEntry>> ListAsync()
        {
            List<UserEntry> users = new List<UserEntry>();

            List<LdapEntry> list = await ldapService.SearchAsync("(&(objectClass=user)(objectCategory=person))");

            foreach (LdapEntry entry in list)
            {
                users.Add(ADResolver<UserEntry>.GetValues(entry));
            }

            return users;
        }

        public async Task<UserEntry?> GetByCNAsync(string cn)
        {
            if (string.IsNullOrEmpty(cn))
                throw new ArgumentNullException(nameof(cn));

            var result = await ldapService.SearchAsync("(&(objectClass=user)(objectCategory=person)(cn=" + cn + "))");
            var entry = result.FirstOrDefault();

            if (entry != null)
                return ADResolver<UserEntry>.GetValues(entry);
            else
                return null;
        }

        public async Task<UserEntry?> AddAsync(UserEntry user, string password)
        {
            return await AddAsync(string.Empty, user, password, false, false, false, false);
        }

        public async Task<UserEntry?> AddAsync(string distinguishedName, UserEntry user, string password)
        {
            return await AddAsync(distinguishedName, user, password, false, false, false, false);
        }

        public async Task<UserEntry?> AddAsync(string distinguishedName, UserEntry user, string password, bool isCannotChangePassword, bool isPasswordNeverExpires, bool isAccountDisabled, bool isMustChangePassword)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrEmpty(user.CN))
                throw new ArgumentNullException(nameof(user.CN));

            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));

            if (string.IsNullOrEmpty(user.UserPrincipalName))
                user.UserPrincipalName = user.SamAccountName + "@" + ldapService.DomainName;

            List<string> attributes = new List<string>
            {
                "displayName",
                "initials",
                "givenName",
                "sn",
                "sAMAccountName",
                "userPrincipalName"
            };

            if (string.IsNullOrEmpty(distinguishedName))
            {
                string cn = "cn=" + user.CN + "," + new ADContainers(ldapService).GetUsersContainer();
                await ldapService.AddAsync(LdapResolver.GetLdapEntry(cn, user, attributes));
            }
            else
            {
                string cn = "cn=" + user.CN + "," + distinguishedName;
                await ldapService.AddAsync(LdapResolver.GetLdapEntry(cn, user, attributes));
            }

            var result = await ldapService.SearchAsync("(&(objectClass=user)(objectCategory=person)(cn=" + user.CN + "))");
            var entry = result.FirstOrDefault();

            if (entry != null)
            {
                UserEntry newUser = ADResolver<UserEntry>.GetValues(entry);

                await ResetPasswordAsync(newUser, password);

                await ChangeUserAccountControlAsync(newUser, isCannotChangePassword, isPasswordNeverExpires, isAccountDisabled);

                if (isMustChangePassword)
                    await MustChangePasswordAsync(newUser);

                return newUser;
            }

            return null;
        }

        public async Task<UserEntry?> ModifyAsync(UserEntry user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrEmpty(user.CN))
                throw new ArgumentNullException(nameof(user.CN));

            if (string.IsNullOrEmpty(user.DistinguishedName))
                throw new ArgumentNullException(nameof(user.DistinguishedName));


            List<string> attributes = new List<string>
            {
                "displayName",
                "initials",
                "givenName",
                "sn",
                "description",
                "physicalDeliveryOfficeName",
                "telephoneNumber",
                "mail",
                "wWWHomePage",
                "streetAddress",
                "postOfficeBox",
                "l",
                "st",
                "postalCode",
                "homePhone",
                "mobile",
                "facsimileTelephoneNumber",
                "title",
                "department",
                "company"
            };

            var result = await ldapService.SearchAsync("(&(objectClass=user)(objectCategory=person)(cn=" + user.CN + "))");
            var entry = result.FirstOrDefault();

            if (entry != null)
            {
                UserEntry oldUser = ADResolver<UserEntry>.GetValues(entry);
                await ldapService.SendRequestAsync(new ModifyRequest(user.DistinguishedName, LdapResolver.GetDirectoryModificationAttributes(user, oldUser, attributes).ToArray()));

                var newUser = await GetByCNAsync(user.CN);
                if (newUser != null)
                    return newUser;
            }

            return null;
        }

        public async Task ResetPasswordAsync(UserEntry user, string password)
        {

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && ldapService.Server == null)
            {
                PrincipalContext context;

                if (ldapService.Credential == null)
                    context = new PrincipalContext(ContextType.Domain);
                else
                    context = new PrincipalContext(ContextType.Domain, ldapService.Credential.UserName, ldapService.Credential.Password);

                using (var identity = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, user.SamAccountName))
                {
                    identity.SetPassword(password);
                    identity.Save();
                }
                context.Dispose();
            }
            else
            {
                if (ldapService.Server != null && ldapService.Server.IsSSL == false)
                    throw new Exception("Require SSL connection to set password");

                var attribute = new DirectoryModificationAttribute()
                {
                    Name = "unicodePwd",
                    LdapModOperation = LdapModOperation.LDAP_MOD_REPLACE
                };

                byte[] encodedBytes = Encoding.Unicode.GetBytes("\"" + password + "\"");
                attribute.Add(encodedBytes);

                await ldapService.SendRequestAsync(new ModifyRequest(user.DistinguishedName, attribute));
            }
        }

        public async Task ChangeUserAccountControlAsync(UserEntry user, bool isCannotChangePassword, bool isPasswordNeverExpires, bool isAccountDisabled)
        {
            UserAccountControls userAccountControl = user.UserControl;

            if (isCannotChangePassword)
                userAccountControl = userAccountControl | UserAccountControls.PASSWD_CANT_CHANGE;
            else
                userAccountControl = userAccountControl & ~UserAccountControls.PASSWD_CANT_CHANGE;

            if (isPasswordNeverExpires)
                userAccountControl = userAccountControl | UserAccountControls.DONT_EXPIRE_PASSWD;
            else
                userAccountControl = userAccountControl & ~UserAccountControls.DONT_EXPIRE_PASSWD;

            if (isAccountDisabled)
                userAccountControl = userAccountControl | UserAccountControls.ACCOUNTDISABLE;
            else
                userAccountControl = userAccountControl & ~UserAccountControls.ACCOUNTDISABLE;

            await ldapService.ModifyPropertyAsync(user.DistinguishedName, "userAccountControl", Convert.ToString((int)userAccountControl));
        }

        public async Task MustChangePasswordAsync(UserEntry user)
        {
            await ldapService.ModifyPropertyAsync(user.DistinguishedName, "pwdlastset", "0");
        }

        public async Task DeleteAsync(UserEntry user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrEmpty(user.DistinguishedName))
                throw new ArgumentNullException(nameof(user.DistinguishedName));

            await ldapService.DeleteAsync(user.DistinguishedName);
        }

        public void Dispose()
        {

        }

    }
}