using LdapForNet;
using NUnit.Framework;
using SysAdmin.ActiveDirectory.Models;
using SysAdmin.ActiveDirectory.Repositories;
using SysAdmin.ActiveDirectory.Services.Ldap;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationsTests
{

    [NonParallelizable]
    public class ContactsRepositoryTests
    {

        public IServer server;
        public ICredential credential;

        [SetUp]
        public void Setup()
        {
            server = TestsSetup.SERVER;
            credential = TestsSetup.CREDENTIAL;
        }

        [Test, Order(1)]
        public async Task GetContactsListTestAsync()
        {
            using (var ldap = new LdapService(server, credential))
            {
                using (var contactsRepository = new ContactsRepository(ldap))
                {
                    List<ContactEntry> contacts = await contactsRepository.ListAsync();
                    Assert.AreEqual(contacts.Count, 0);
                }
            }
        }

        [Test, Order(2)]
        public async Task AddContactTestAsync()
        {
            using (var ldap = new LdapService(server, credential))
            {
                using (var contactsRepository = new ContactsRepository(ldap))
                {
                    Assert.IsNull(contactsRepository.GetByCNAsync("test.contact").Result);

                    ContactEntry contact = new ContactEntry()
                    {
                        CN = "test.contact",
                        DisplayName = "Test Contact",
                        FirstName = "Test",
                        LastName = "Contact"
                    };

                    await contactsRepository.AddAsync(contact);

                    Assert.IsNotNull(contactsRepository.GetByCNAsync("test.contact").Result);
                }
            }
        }

        [Test, Order(3)]
        public async Task ModifyContactTestAsync()
        {
            using (var ldap = new LdapService(server, credential))
            {
                using (var contactsRepository = new ContactsRepository(ldap))
                {
                    var contact = await contactsRepository.GetByCNAsync("test.contact");

                    if (contact != null)
                    {
                        contact.Description = "Test description";
                        await contactsRepository.ModifyAsync(contact);
                    }
                    else
                    {
                        Assert.Fail();
                    }

                    var modifyContact = await contactsRepository.GetByCNAsync("test.contact");

                    if (modifyContact != null)
                    {
                        Assert.AreEqual(modifyContact.Description, "Test description");
                    }
                    else
                    {
                        Assert.Fail();
                    }
                }
            }
        }

        [Test, Order(4)]
        public async Task DeleteContactTestAsync()
        {
            using (var ldap = new LdapService(server, credential))
            {
                using (var contactsRepository = new ContactsRepository(ldap))
                {
                    var contact = await contactsRepository.GetByCNAsync("test.contact");

                    if (contact != null)
                        await contactsRepository.DeleteAsync(contact);
                    else
                        Assert.Fail();

                    Assert.IsNull(contactsRepository.GetByCNAsync("test.contact").Result);
                }
            }
        }

    }

}