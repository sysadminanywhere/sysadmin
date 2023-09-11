using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sysadmin.ActiveDirectory.Services.Ldap
{
    public class SearchService
    {

        readonly LdapConnection ldapConnection;

        public SearchService(LdapConnection ldapConnection) 
        {
            this.ldapConnection = ldapConnection;
        }

        public SearchResultEntryCollection Search(string distinguishedName, string searchFilter, SearchScope searchScope = SearchScope.Subtree, string[]? parameters = null)
        {
            SearchRequest searchRequest = new SearchRequest(distinguishedName, searchFilter, searchScope, parameters);
            var response = (SearchResponse)ldapConnection.SendRequest(searchRequest);

            //string userDN = response.Entries[0].Attributes["DistinguishedName"][0].ToString();

            return response.Entries;
        }

    }
}
