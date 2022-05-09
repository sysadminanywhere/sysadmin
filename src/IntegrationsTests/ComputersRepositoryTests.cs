using LdapForNet;
using NUnit.Framework;
using SysAdmin.ActiveDirectory.Models;
using SysAdmin.ActiveDirectory.Repositories;
using SysAdmin.ActiveDirectory.Services.Ldap;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntegrationsTests
{

    [NonParallelizable]
    public class ComputersRepositoryTests
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
        public async Task GetComputersListTestAsync()
        {
            using (var ldap = new LdapService(server, credential))
            {
                using (var computersRepository = new ComputersRepository(ldap))
                {
                    List<ComputerEntry> computers = await computersRepository.ListAsync();
                    Assert.Greater(computers.Count, 0);
                }
            }
        }

        [Test, Order(2)]
        public async Task AddComputerTestAsync()
        {
            using (var ldap = new LdapService(server, credential))
            {
                using (var computersRepository = new ComputersRepository(ldap))
                {
                    Assert.IsNull(computersRepository.GetByCNAsync("test.computer").Result);

                    ComputerEntry computer = new ComputerEntry()
                    {
                        CN = "test.computer",
                        Location = "Home",
                        SamAccountName = "test.computer"
                    };

                    await computersRepository.AddAsync(computer, true);

                    Assert.IsNotNull(computersRepository.GetByCNAsync("test.computer").Result);
                }
            }
        }

        [Test, Order(3)]
        public async Task ModifyComputerTestAsync()
        {
            using (var ldap = new LdapService(server, credential))
            {
                using (var computersRepository = new ComputersRepository(ldap))
                {
                    var computer = await computersRepository.GetByCNAsync("test.computer");

                    if (computer != null)
                    {
                        computer.Description = "Test description";
                        await computersRepository.ModifyAsync(computer);
                    }
                    else
                    {
                        Assert.Fail();
                    }

                    var modifyComputer = await computersRepository.GetByCNAsync("test.computer");

                    if (modifyComputer != null)
                    {
                        Assert.AreEqual(modifyComputer.Description, "Test description");
                    }
                    else
                    {
                        Assert.Fail();
                    }
                }
            }
        }

        [Test, Order(4)]
        public async Task DeleteComputerTestAsync()
        {
            using (var ldap = new LdapService(server, credential))
            {
                using (var computersRepository = new ComputersRepository(ldap))
                {
                    var computer = await computersRepository.GetByCNAsync("test.computer");

                    if (computer != null)
                        await computersRepository.DeleteAsync(computer);
                    else
                        Assert.Fail();

                    Assert.IsNull(computersRepository.GetByCNAsync("test.computer").Result);
                }
            }
        }

    }

}