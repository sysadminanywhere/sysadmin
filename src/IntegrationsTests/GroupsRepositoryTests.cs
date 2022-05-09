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
    public class GroupsRepositoryTests
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
        public async Task GetGroupsListTestAsync()
        {
            using (var ldap = new LdapService(server, credential))
            {
                using (var groupsRepository = new GroupsRepository(ldap))
                {
                    List<GroupEntry> groups = await groupsRepository.ListAsync();
                    Assert.Greater(groups.Count, 0);
                }
            }
        }

        [Test, Order(2)]
        public async Task AddGroupTestAsync()
        {
            using (var ldap = new LdapService(server, credential))
            {
                using (var groupsRepository = new GroupsRepository(ldap))
                {
                    Assert.IsNull(groupsRepository.GetByCNAsync("test.group").Result);

                    GroupEntry group = new GroupEntry()
                    {
                        CN = "test.group"
                    };

                    await groupsRepository.AddAsync(group);

                    Assert.IsNotNull(groupsRepository.GetByCNAsync("test.group").Result);
                }
            }
        }

        [Test, Order(3)]
        public async Task ModifyGroupTestAsync()
        {
            using (var ldap = new LdapService(server, credential))
            {
                using (var groupsRepository = new GroupsRepository(ldap))
                {
                    var group = await groupsRepository.GetByCNAsync("test.group");

                    if (group != null)
                    {
                        group.Description = "Test description";
                        await groupsRepository.ModifyAsync(group);
                    }
                    else
                    {
                        Assert.Fail();
                    }

                    var modifyGroup = await groupsRepository.GetByCNAsync("test.group");

                    if (modifyGroup != null)
                    {
                        Assert.AreEqual(modifyGroup.Description, "Test description");
                    }
                    else
                    {
                        Assert.Fail();
                    }
                }
            }
        }

        [Test, Order(4)]
        public async Task AddMemberToGroupTestAsync()
        {
            using (var ldap = new LdapService(server, credential))
            {
                using (var groupsRepository = new GroupsRepository(ldap))
                {
                    var group = await groupsRepository.GetByCNAsync("test.group");

                    if (group != null)
                    {
                        await groupsRepository.AddMemberAsync(group, "CN=Administrator,CN=Users,DC=example,DC=com");
                        await groupsRepository.AddMemberAsync(group, "CN=admin,CN=Users,DC=example,DC=com");
                    }
                    else
                    {
                        Assert.Fail();
                    }

                    var modifyGroup = await groupsRepository.GetByCNAsync("test.group");

                    if (modifyGroup != null)
                    {
                        Assert.True(modifyGroup.Members.Contains("CN=Administrator,CN=Users,DC=example,DC=com"));
                        Assert.True(modifyGroup.Members.Contains("CN=admin,CN=Users,DC=example,DC=com"));
                    }
                    else
                    {
                        Assert.Fail();
                    }
                }
            }
        }

        [Test, Order(5)]
        public async Task DeleteMemberFromGroupTestAsync()
        {
            using (var ldap = new LdapService(server, credential))
            {
                using (var groupsRepository = new GroupsRepository(ldap))
                {
                    var group = await groupsRepository.GetByCNAsync("test.group");

                    if (group != null)
                    {
                        await groupsRepository.DeleteMemberAsync(group, "CN=Administrator,CN=Users,DC=example,DC=com");
                        await groupsRepository.DeleteMemberAsync(group, "CN=admin,CN=Users,DC=example,DC=com");
                    }
                    else
                    {
                        Assert.Fail();
                    }

                    var modifyGroup = await groupsRepository.GetByCNAsync("test.group");

                    if (modifyGroup != null)
                    {
                        Assert.IsNull(modifyGroup.Members);
                    }
                    else
                    {
                        Assert.Fail();
                    }
                }
            }
        }

        [Test, Order(6)]
        public async Task DeleteGroupTestAsync()
        {
            using (var ldap = new LdapService(server, credential))
            {
                using (var groupsRepository = new GroupsRepository(ldap))
                {
                    var group = await groupsRepository.GetByCNAsync("test.group");

                    if (group != null)
                        await groupsRepository.DeleteAsync(group);
                    else
                        Assert.Fail();

                    Assert.IsNull(groupsRepository.GetByCNAsync("test.group").Result);
                }
            }
        }

    }

}