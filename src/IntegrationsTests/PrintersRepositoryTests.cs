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
    public class PrintersRepositoryTests
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
        public async Task GetPrintersListTestAsync()
        {
            using (var ldap = new LdapService(server, credential))
            {
                using (var printersRepository = new PrintersRepository(ldap))
                {
                    List<PrinterEntry> printers = await printersRepository.ListAsync();
                    Assert.AreEqual(printers.Count, 0);
                }
            }
        }

    }

}