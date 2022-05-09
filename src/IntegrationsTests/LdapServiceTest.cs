using LdapForNet;
using NUnit.Framework;
using SysAdmin.ActiveDirectory.Services.Ldap;
using System.Collections.Generic;
using System.Threading.Tasks;
using static LdapForNet.Native.Native;

namespace IntegrationsTests
{
    [NonParallelizable]
    public class LdapServiceTest
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
        public void ConnectionTest()
        {
            using (var ldap = new LdapService(server, credential))
            {
                Assert.True(ldap.IsConnected);
            }
        }

        [Test, Order(2)]
        public async Task SearchTestAsync()
        {
            using (var ldap = new LdapService(server, credential))
            {
                List<LdapEntry> list = await ldap.SearchAsync("(objectClass=computer)");
                Assert.Greater(list.Count, 0);
            }
        }

        [Test, Order(3)]
        public async Task AddEntryTestAsync()
        {
            using (var ldap = new LdapService(server, credential))
            {
                await ldap.AddAsync(new LdapEntry
                {
                    Dn = "cn=test," + ldap.DefaultNamingContext,
                    Attributes = new Dictionary<string, List<string>>
                    {
                        {"sn", new List<string> {"Testonsson"}},
                        {"objectclass", new List<string> {"User"}},
                        {"givenName", new List<string> {"Test"}},
                        {"description", new List<string> {"Test user"}}
                    }
                });
                List<LdapEntry> list = await ldap.SearchAsync("(&(objectClass=User)(cn=test))");
                Assert.AreEqual(list.Count, 1);
            }
        }

        [Test, Order(4)]
        public async Task ModifyEntryTestAsync()
        {
            using (var ldap = new LdapService(server, credential))
            {
                await ldap.ModifyAsync(new LdapModifyEntry
                {
                    Dn = "cn=test," + ldap.DefaultNamingContext,
                    Attributes = new List<LdapModifyAttribute>
                    {
                        new LdapModifyAttribute
                        {
                            LdapModOperation = LdapModOperation.LDAP_MOD_ADD,
                            Type = "displayName",
                            Values = new List<string> { "Test Testonsson" }
                        },
                        new LdapModifyAttribute
                        {
                            LdapModOperation = LdapModOperation.LDAP_MOD_DELETE,
                            Type = "description",
                            Values = new List<string> { "Test user" }
                        }
                    }
                });

                List<LdapEntry> list = await ldap.SearchAsync("(&(objectClass=User)(cn=test))");
                Assert.AreEqual(list.Count, 1);
                Assert.IsFalse(list[0].Attributes.ContainsKey("description"));
                Assert.AreEqual(list[0].Attributes["displayName"], new List<string> { "Test Testonsson" });
            }
        }

        [Test, Order(5)]
        public async Task DeleteEntryTestAsync()
        {
            using (var ldap = new LdapService(server, credential))
            {
                await ldap.DeleteAsync("cn=test," + ldap.DefaultNamingContext);

                List<LdapEntry> list = await ldap.SearchAsync("(&(objectClass=User)(cn=test))");
                Assert.AreEqual(list.Count, 0);
            }
        }

    }
}