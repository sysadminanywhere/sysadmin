using CommunityToolkit.Mvvm.ComponentModel;
using LdapForNet;
using SysAdmin.ActiveDirectory;
using SysAdmin.ActiveDirectory.Models;
using SysAdmin.ActiveDirectory.Services.Ldap;
using SysAdmin.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sysadmin.ViewModels
{
    public partial class DashboardViewModel : ViewModel
    {
        [ObservableProperty]
        private string _domainName;

        [ObservableProperty]
        private string _distinguishedName;

        [ObservableProperty]
        private int _computersCount;

        [ObservableProperty]
        private int _usersCount;

        [ObservableProperty]
        private int _groupsCount;

        [ObservableProperty]
        private int _contactsCount;

        [ObservableProperty]
        private int _printersCount;

        [ObservableProperty]
        private IEnumerable<AuditItem> _auditList;

        public override async void OnNavigatedTo()
        {
            await GetAsync();
        }

        public async Task GetAsync()
        {

            List<AuditItem> list = new List<AuditItem>();

            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    DomainName = ldap.DomainName.ToUpper();
                    DistinguishedName = ldap.DefaultNamingContext;

                    var computers = await ldap.SearchAsync("(objectClass=computer)");
                    ComputersCount = computers.Count;

                    var users = await ldap.SearchAsync("(&(objectClass=user)(objectCategory=person))");
                    UsersCount = users.Count;

                    var groups = await ldap.SearchAsync("(objectClass=group)");
                    GroupsCount = groups.Count;

                    var printers = await ldap.SearchAsync("(objectClass=printQueue)");
                    PrintersCount = printers.Count;

                    var contacts = await ldap.SearchAsync("(&(objectClass=contact)(objectCategory=person))");
                    ContactsCount = contacts.Count;

                    AuditList = await ldap.AuditListAsync();
                }
            });

        }

    }

}