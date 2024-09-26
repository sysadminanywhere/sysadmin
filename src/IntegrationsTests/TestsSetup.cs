using NUnit.Framework;
using SysAdmin.ActiveDirectory.Services.Ldap;

namespace IntegrationsTests
{

    [SetUpFixture]
    public class TestsSetup
    {

        public static IServer SERVER;
        public static ICredential CREDENTIAL;

        [OneTimeSetUp]
        public void GlobalSetup()
        {
            SERVER = new SecureServer("192.168.245.129");

            CREDENTIAL = new Credential()
            {
                UserName = "admin",
                Password = "Secret2#"
            };
        }

        [OneTimeTearDown]
        public void GlobalTeardown()
        {
        }
    }
}