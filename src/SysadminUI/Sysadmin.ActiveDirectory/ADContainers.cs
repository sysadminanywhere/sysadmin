using SysAdmin.ActiveDirectory.Services.Ldap;

namespace SysAdmin.ActiveDirectory
{
    public class ADContainers
    {
        private const string ContainerMicrosoft = "B:32:F4BE92A4C777485E878E9421D53087DB:";                 //CN=Microsoft,CN=Program Data,DC=example,DC=com
        private const string ContainerProgramData = "B:32:09460C08AE1E4A4EA0F64AEE7DAA1E5A:";               //CN=Program Data,DC=example,DC=com
        private const string ContainerForeignSecurityPrincipals = "B:32:22B70C67D56E4EFB91E9300FCA3DC1AA:"; //CN=ForeignSecurityPrincipals,DC=example,DC=com
        private const string ContainerDeletedObjects = "B:32:18E2EA80684F11D2B9AA00C04F79F805:";            //CN=Deleted Objects,DC=example,DC=com
        private const string ContainerInfrastructure = "B:32:2FBAC1870ADE11D297C400C04FD8D5CD:";            //CN=Infrastructure,DC=example,DC=com
        private const string ContainerLostAndFound = "B:32:AB8153B7768811D1ADED00C04FD8D5CD:";              //CN=LostAndFound,DC=example,DC=com
        private const string ContainerSystem = "B:32:AB1D30F3768811D1ADED00C04FD8D5CD:";                    //CN=System,DC=example,DC=com
        private const string ContainerDomainControllers = "B:32:A361B2FFFFD211D1AA4B00C04FD7D83A:";         //OU=Domain Controllers,DC=example,DC=com
        private const string ContainerComputers = "B:32:AA312825768811D1ADED00C04FD8D5CD:";                 //CN=Computers,DC=example,DC=com
        private const string ContainerUsers = "B:32:A9D1CA15768811D1ADED00C04FD8D5CD:";                     //CN=Users,DC=example,DC=com
        private const string ContainerNTDSQuotas = "B:32:6227F0AF1FC2410D8E3BB10615BB5B0F:";                //CN=NTDS Quotas,DC=example,DC=com

        private List<string> wellKnownObjects;

        public ADContainers(LdapService ldapService)
        {
            wellKnownObjects = ldapService.WellKnownObjectsAsync().Result;
        }

        public string GetComputersContainer()
        {
            return wellKnownObjects.First(c => c.StartsWith(ContainerComputers)).Replace(ContainerComputers, string.Empty);
        }

        public string GetUsersContainer()
        {
            return wellKnownObjects.First(c => c.StartsWith(ContainerUsers)).Replace(ContainerUsers, string.Empty);
        }

    }

}