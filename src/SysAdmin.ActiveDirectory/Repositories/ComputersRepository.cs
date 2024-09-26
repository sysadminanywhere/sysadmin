using LdapForNet;
using SysAdmin.ActiveDirectory.Models;
using SysAdmin.ActiveDirectory.Services.Ldap;

namespace SysAdmin.ActiveDirectory.Repositories
{
    public class ComputersRepository : IDisposable
    {

        private LdapService ldapService;

        public ComputersRepository(LdapService ldapService)
        {
            if (ldapService == null)
                throw new ArgumentNullException(nameof(ldapService));

            this.ldapService = ldapService;
        }

        public async Task<List<ComputerEntry>> ListAsync()
        {
            List<ComputerEntry> computers = new List<ComputerEntry>();

            List<LdapEntry> list = await ldapService.SearchAsync("(objectClass=computer)");

            foreach (LdapEntry entry in list)
            {
                computers.Add(ADResolver<ComputerEntry>.GetValues(entry));
            }

            return computers;
        }

        public async Task<ComputerEntry?> GetByCNAsync(string cn)
        {
            if (string.IsNullOrEmpty(cn))
                throw new ArgumentNullException(nameof(cn));

            var result = await ldapService.SearchAsync("(&(objectClass=computer)(cn=" + cn + "))");
            var entry = result.FirstOrDefault();

            if (entry != null)
                return ADResolver<ComputerEntry>.GetValues(entry);
            else
                return null;
        }

        public async Task<ComputerEntry?> AddAsync(ComputerEntry computer, bool isEnabled)
        {
            if (computer == null)
                throw new ArgumentNullException(nameof(computer));

            return await AddAsync(string.Empty, computer, isEnabled);
        }

        public async Task<ComputerEntry?> AddAsync(string distinguishedName, ComputerEntry computer, bool isEnabled)
        {
            if (computer == null)
                throw new ArgumentNullException(nameof(computer));

            if (string.IsNullOrEmpty(computer.CN))
                throw new ArgumentNullException(nameof(computer.CN));


            List<string> attributes = new List<string>
            {
                "description",
                "location",
                "sAMAccountName"
            };

            if (string.IsNullOrEmpty(computer.SamAccountName))
                computer.SamAccountName = computer.CN;

            if (string.IsNullOrEmpty(distinguishedName))
            {
                string cn = "cn=" + computer.CN + "," + new ADContainers(ldapService).GetUsersContainer();
                await ldapService.AddAsync(LdapResolver.GetLdapEntry(cn, computer, attributes));
            }
            else
            {
                string cn = "cn=" + computer.CN + "," + distinguishedName;
                await ldapService.AddAsync(LdapResolver.GetLdapEntry(cn, computer, attributes));
            }

            var result = await ldapService.SearchAsync("(&(objectClass=computer)(cn=" + computer.CN + "))");
            var entry = result.FirstOrDefault();

            if (entry != null)
            {
                ComputerEntry newComputer = ADResolver<ComputerEntry>.GetValues(entry);

                UserAccountControls userAccountControl = newComputer.UserControl;

                if (!isEnabled)
                {
                    if ((userAccountControl & UserAccountControls.ACCOUNTDISABLE) != UserAccountControls.ACCOUNTDISABLE)
                        userAccountControl = userAccountControl & UserAccountControls.ACCOUNTDISABLE;
                }
                else
                {
                    if ((userAccountControl & UserAccountControls.ACCOUNTDISABLE) == UserAccountControls.ACCOUNTDISABLE)
                        userAccountControl = userAccountControl & ~UserAccountControls.ACCOUNTDISABLE;
                }

                await ldapService.ModifyPropertyAsync(newComputer.DistinguishedName, "userAccountControl", Convert.ToString((int)userAccountControl));

                return newComputer;
            }

            return null;
        }

        public async Task<ComputerEntry?> ModifyAsync(ComputerEntry computer)
        {
            if (computer == null)
                throw new ArgumentNullException(nameof(computer));

            if (string.IsNullOrEmpty(computer.CN))
                throw new ArgumentNullException(nameof(computer.CN));

            if (string.IsNullOrEmpty(computer.DistinguishedName))
                throw new ArgumentNullException(nameof(computer.DistinguishedName));

            List<string> attributes = new List<string>
            {
                "description",
                "location"
            };

            var result = await ldapService.SearchAsync("(&(objectClass=computer)(cn=" + computer.CN + "))");
            var entry = result.FirstOrDefault();

            if (entry != null)
            {
                ComputerEntry oldComputer = ADResolver<ComputerEntry>.GetValues(entry);
                await ldapService.SendRequestAsync(new ModifyRequest(computer.DistinguishedName, LdapResolver.GetDirectoryModificationAttributes(computer, oldComputer, attributes).ToArray()));

                var newComputer = await GetByCNAsync(computer.CN);
                if (newComputer != null)
                    return newComputer;
            }
            return null;
        }

        public async Task DeleteAsync(ComputerEntry computer)
        {
            if (computer == null)
                throw new ArgumentNullException(nameof(computer));

            if (string.IsNullOrEmpty(computer.DistinguishedName))
                throw new ArgumentNullException(nameof(computer.DistinguishedName));

            await ldapService.DeleteAsync(computer.DistinguishedName);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            ldapService?.Dispose();
        }

    }
}