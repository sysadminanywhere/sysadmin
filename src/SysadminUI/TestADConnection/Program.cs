using System.DirectoryServices.Protocols;

internal class Program
{
    private static void Main(string[] args)
    {
        var ldapConnection = new LdapConnection(
            new LdapDirectoryIdentifier("192.168.245.129", 389), 
            new System.Net.NetworkCredential("administrator", "Secret2#"));

        Console.WriteLine("Connected");

        Console.WriteLine("Users:");
        Search(ldapConnection, "(objectClass=user)");

        Console.WriteLine("Computers:");
        Search(ldapConnection, "(objectClass=computer)");

        Console.WriteLine("Groups:");
        Search(ldapConnection, "(objectClass=group)");

        Console.ReadLine();
    }

    private static void Search(LdapConnection ldapConnection, string filter)
    {
        SearchRequest searchRequest = new SearchRequest("DC=example,DC=com", filter, SearchScope.Subtree, new string[] { "DistinguishedName" });

        var response = (SearchResponse)ldapConnection.SendRequest(searchRequest);
        string dn = response.Entries[0].Attributes["DistinguishedName"][0].ToString();

        Console.WriteLine(dn);
    }

}