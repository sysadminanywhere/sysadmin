using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using SysAdmin.ActiveDirectory.Services.Ldap;
using SysAdmin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysAdmin.ViewModels
{
    public class HomeViewModel : ObservableObject
    {

        public string DomainName { get; set; }
        public string DistinguishedName { get; set; }

        public int ComputersCount { get; set; }
        public int UsersCount { get; set; }
        public int GroupsCount { get; set; }
        public int ContactsCount { get; set; }
        public int PrintersCount { get; set; }


        IBusyService busyService = App.Current.Services.GetService<IBusyService>();

        public async Task GetAsync()
        {
            busyService.Busy();

            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    DomainName = ldap.DomainName.ToUpper();
                    DistinguishedName = ldap.DefaultNamingContext;

                    var computers = await ldap.SearchAsync("(objectClass=computer)");
                    ComputersCount = computers.Count();

                    var users = await ldap.SearchAsync("(&(objectClass=user)(objectCategory=person))");
                    UsersCount = users.Count();

                    var groups = await ldap.SearchAsync("(objectClass=group)");
                    GroupsCount = groups.Count();

                    var printers = await ldap.SearchAsync("(objectClass=printQueue)");
                    PrintersCount = printers.Count();

                    var contacts = await ldap.SearchAsync("(&(objectClass=contact)(objectCategory=person))");
                    ContactsCount = contacts.Count();
                }
            });

            OnPropertyChanged(nameof(DomainName));
            OnPropertyChanged(nameof(DistinguishedName));

            OnPropertyChanged(nameof(ComputersCount));
            OnPropertyChanged(nameof(UsersCount));
            OnPropertyChanged(nameof(GroupsCount));
            OnPropertyChanged(nameof(PrintersCount));
            OnPropertyChanged(nameof(ContactsCount));

            busyService.Idle();
        }

    }
}