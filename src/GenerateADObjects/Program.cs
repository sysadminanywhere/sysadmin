using Bogus;
using SysAdmin.ActiveDirectory.Models;
using SysAdmin.ActiveDirectory.Repositories;
using SysAdmin.ActiveDirectory.Services.Ldap;

internal class Program
{

    private static IServer Server = null;
    private static ICredential Credential = null;


    private static async Task Main(string[] args)
    {

        //Server = new Server("192.168.245.129", 389);
        //Credential = new Credential("admin", "Secret2#");

        try
        {
            for (int i = 0; i < 999; i++)
            {
                await GenerateUser("OU=Users,OU=Test1,DC=example,DC=com");
            }

            //await GenerateContact("OU=Contacts,OU=Test1,DC=example,DC=com");
            //await GenerateComputer("OU=Computers,OU=Test1,DC=example,DC=com");
        } catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        Console.WriteLine("Press any key...");
        Console.ReadLine();
    }

    private static async Task GenerateUser(string distinguishedName)
    {
        var testUsers = new Faker<UserEntry>()

            .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
            .RuleFor(u => u.LastName, (f, u) => f.Name.LastName())
            .RuleFor(u => u.SamAccountName, (f, u) => f.Internet.UserName(u.FirstName, u.LastName))
            .RuleFor(u => u.DisplayName, (f, u) => u.FirstName + " " + u.LastName)

            .FinishWith((f, u) =>
            {
                Console.WriteLine(u.DisplayName);
            });

        using (var ldap = new LdapService(Server, Credential))
        {
            using (var usersRepository = new UsersRepository(ldap))
            {
                var user = testUsers.Generate();
                user.DistinguishedName = distinguishedName;

                if (string.IsNullOrEmpty(user.CN))
                    user.CN = user.DisplayName;

                if (string.IsNullOrEmpty(user.Name))
                    user.Name = user.DisplayName;

                await usersRepository.AddAsync(distinguishedName, user);
            }
        }
    }

    private static async Task GenerateContact(string distinguishedName)
    {
        var testContacts = new Faker<ContactEntry>()

            .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
            .RuleFor(u => u.LastName, (f, u) => f.Name.LastName())
            .RuleFor(u => u.DisplayName, (f, u) => u.FirstName + " " + u.LastName)

            .FinishWith((f, u) =>
            {
                Console.WriteLine(u.DisplayName);
            });

        using (var ldap = new LdapService(Server, Credential))
        {
            using (var contactsRepository = new ContactsRepository(ldap))
            {
                var contact = testContacts.Generate();
                contact.DistinguishedName = distinguishedName;

                if (string.IsNullOrEmpty(contact.CN))
                    contact.CN = contact.DisplayName;

                if (string.IsNullOrEmpty(contact.Name))
                    contact.Name = contact.DisplayName;

                await contactsRepository.AddAsync(contact);
            }
        }
    }

    private static async Task GenerateComputer(string distinguishedName)
    {
        var testComputers = new Faker<ComputerEntry>()

            .RuleFor(u => u.CN, (f, u) => f.Random.Word())
            .RuleFor(u => u.Description, (f, u) => f.Random.Words())

            .FinishWith((f, u) =>
            {
                Console.WriteLine(u.Name);
            });

        using (var ldap = new LdapService(Server, Credential))
        {
            using (var computersRepository = new ComputersRepository(ldap))
            {
                var computer = testComputers.Generate();
                computer.DistinguishedName = distinguishedName;

                await computersRepository.AddAsync(computer, false);
            }
        }
    }

}