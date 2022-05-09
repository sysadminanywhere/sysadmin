using SysAdmin.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysAdmin.ActiveDirectory.Models
{

    /*

    dnsHostName: WIN-F189Q2V238G.example.com
    serverName: CN=WIN-F189Q2V238G,CN=Servers,CN=Default-First-Site-Name,CN=Sites,CN=Configuration,DC=example,DC=com
    configurationNamingContext: CN=Configuration,DC=example,DC=com
    currentTime: 20220413125701.0Z
    defaultNamingContext: DC=example,DC=com
    domainControllerFunctionality: 7
    domainFunctionality: 7
    dsServiceName: CN=NTDS Settings,CN=WIN-F189Q2V238G,CN=Servers,CN=Default-First-Site-Name,CN=Sites,CN=Configuration,DC=example,DC=com
    forestFunctionality: 7
    highestCommittedUSN: 61487
    isGlobalCatalogReady: TRUE
    isSynchronized: TRUE
    ldapServiceName: example.com:win-f189q2v238g$@EXAMPLE.COM

    namingContexts: CN=Configuration,DC=example,DC=com
    namingContexts: CN=Schema,CN=Configuration,DC=example,DC=com
    namingContexts: DC=DomainDnsZones,DC=example,DC=com
    namingContexts: DC=example,DC=com
    namingContexts: DC=ForestDnsZones,DC=example,DC=com

    rootDomainNamingContext: DC=example,DC=com
    schemaNamingContext: CN=Schema,CN=Configuration,DC=example,DC=com
    subschemaSubentry: CN=Aggregate,CN=Schema,CN=Configuration,DC=example,DC=com
    
    */


    public class RootDseEntry : IADEntry
    {
        [AD("cn", true)]
        public string CN { get; set; }

        [AD("objectclass", true)]
        public List<string> ObjectClass { get; set; }

    }
}
