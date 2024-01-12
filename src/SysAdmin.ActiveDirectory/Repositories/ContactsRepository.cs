using LdapForNet;
using SysAdmin.ActiveDirectory.Models;
using SysAdmin.ActiveDirectory.Services.Ldap;

namespace SysAdmin.ActiveDirectory.Repositories
{
    public class ContactsRepository : IDisposable
    {

        private LdapService ldapService;

        public ContactsRepository(LdapService ldapService)
        {
            if (ldapService == null)
                throw new ArgumentNullException(nameof(ldapService));

            this.ldapService = ldapService;
        }

        public async Task<List<ContactEntry>> ListAsync()
        {
            List<ContactEntry> contacts = new List<ContactEntry>();

            List<LdapEntry> list = await ldapService.SearchAsync("(&(objectClass=contact)(objectCategory=person))");

            foreach (LdapEntry entry in list)
            {
                contacts.Add(ADResolver<ContactEntry>.GetValues(entry));
            }

            return contacts;
        }

        public async Task<ContactEntry?> GetByCNAsync(string cn)
        {
            if (string.IsNullOrEmpty(cn))
                throw new ArgumentNullException(nameof(cn));

            var result = await ldapService.SearchAsync("(&(objectClass=contact)(objectCategory=person)(cn=" + cn + "))");
            var entry = result.FirstOrDefault();

            if (entry != null)
                return ADResolver<ContactEntry>.GetValues(entry);
            else
                return null;
        }

        public async Task<ContactEntry?> AddAsync(ContactEntry contact)
        {
            return await AddAsync(string.Empty, contact);
        }

        public async Task<ContactEntry?> AddAsync(string distinguishedName, ContactEntry contact)
        {
            if (contact == null)
                throw new ArgumentNullException(nameof(contact));

            if (string.IsNullOrEmpty(contact.CN))
                throw new ArgumentNullException(nameof(contact.CN));

            List<string> attributes = new List<string>
            {
                "displayName",
                "initials",
                "givenName",
                "sn"
            };

            if (string.IsNullOrEmpty(distinguishedName))
            {
                string cn = "cn=" + contact.CN + "," + new ADContainers(ldapService).GetUsersContainer();
                await ldapService.AddAsync(LdapResolver.GetLdapEntry(cn, contact, attributes));
            }
            else
            {
                string cn = "cn=" + contact.CN + "," + distinguishedName;
                await ldapService.AddAsync(LdapResolver.GetLdapEntry(cn, contact, attributes));
            }

            var result = await ldapService.SearchAsync("(&(objectClass=contact)(objectCategory=person)(cn=" + contact.CN + "))");
            var entry = result.FirstOrDefault();

            if (entry != null)
                return ADResolver<ContactEntry>.GetValues(entry);
            else
                return null;
        }

        public async Task<ContactEntry?> ModifyAsync(ContactEntry contact)
        {
            if (contact == null)
                throw new ArgumentNullException(nameof(contact));

            if (string.IsNullOrEmpty(contact.CN))
                throw new ArgumentNullException(nameof(contact.CN));

            if (string.IsNullOrEmpty(contact.DistinguishedName))
                throw new ArgumentNullException(nameof(contact.DistinguishedName));

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

            var result = await ldapService.SearchAsync("(&(objectClass=contact)(objectCategory=person)(cn=" + contact.CN + "))");
            var entry = result.FirstOrDefault();

            if (entry != null)
            {
                ContactEntry oldContact = ADResolver<ContactEntry>.GetValues(entry);
                await ldapService.SendRequestAsync(new ModifyRequest(contact.DistinguishedName, LdapResolver.GetDirectoryModificationAttributes(contact, oldContact, attributes).ToArray()));

                var newContact = await GetByCNAsync(contact.CN);
                if (newContact != null)
                    return newContact;
            }

            return null;
        }

        public async Task DeleteAsync(ContactEntry contact)
        {
            if (contact == null)
                throw new ArgumentNullException(nameof(contact));

            if (string.IsNullOrEmpty(contact.DistinguishedName))
                throw new ArgumentNullException(nameof(contact.DistinguishedName));

            await ldapService.DeleteAsync(contact.DistinguishedName);
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