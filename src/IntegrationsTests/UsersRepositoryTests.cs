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
    public class UsersRepositoryTests
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
        public async Task GetUsersListTestAsync()
        {
            using (var ldap = new LdapService(server, credential))
            {
                using (var usersRepository = new UsersRepository(ldap))
                {
                    List<UserEntry> users = await usersRepository.ListAsync();
                    Assert.Greater(users.Count, 0);
                }
            }
        }

        [Test, Order(2)]
        public async Task AddUserTestAsync()
        {
            using (var ldap = new LdapService(server, credential))
            {
                using (var usersRepository = new UsersRepository(ldap))
                {
                    Assert.IsNull(usersRepository.GetByCNAsync("test.user").Result);

                    UserEntry user = new UserEntry()
                    {
                        CN = "test.user",
                        DisplayName = "Test User",
                        FirstName = "Test",
                        LastName = "User",
                        SamAccountName = "test.user",
                        UserPrincipalName = "test.user@example.com"
                    };

                    await usersRepository.AddAsync(user, "aaa111#");

                    Assert.IsNotNull(usersRepository.GetByCNAsync("test.user").Result);
                }
            }
        }

        [Test, Order(3)]
        public async Task ModifyUserTestAsync()
        {
            using (var ldap = new LdapService(server, credential))
            {
                using (var usersRepository = new UsersRepository(ldap))
                {
                    var user = await usersRepository.GetByCNAsync("test.user");

                    if (user != null)
                    {
                        user.Description = "Test description";
                        await usersRepository.ModifyAsync(user);
                    }
                    else
                    {
                        Assert.Fail();
                    }

                    List<UserEntry> usersAfter = await usersRepository.ListAsync();

                    var modifyUser = usersAfter.FirstOrDefault(c => c.CN == "test.user");

                    if (modifyUser != null)
                    {
                        Assert.AreEqual(modifyUser.Description, "Test description");
                    }
                    else
                    {
                        Assert.Fail();
                    }
                }
            }
        }

        [Test, Order(4)]
        public async Task DeleteUserTestAsync()
        {
            using (var ldap = new LdapService(server, credential))
            {
                using (var usersRepository = new UsersRepository(ldap))
                {
                    var user = await usersRepository.GetByCNAsync("test.user");

                    if (user != null)
                        await usersRepository.DeleteAsync(user);
                    else
                        Assert.Fail();

                    Assert.IsNull(usersRepository.GetByCNAsync("test.user").Result);
                }
            }
        }

    }

}