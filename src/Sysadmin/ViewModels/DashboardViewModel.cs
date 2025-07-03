using CommunityToolkit.Mvvm.ComponentModel;
using LdapForNet;
using SysAdmin.ActiveDirectory;
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
                    list.AddRange(CreateAudit(computers));
                    ComputersCount = computers.Count;

                    var users = await ldap.SearchAsync("(&(objectClass=user)(objectCategory=person))");
                    list.AddRange(CreateAudit(users));
                    UsersCount = users.Count;

                    var groups = await ldap.SearchAsync("(objectClass=group)");
                    list.AddRange(CreateAudit(groups));
                    GroupsCount = groups.Count;

                    var printers = await ldap.SearchAsync("(objectClass=printQueue)");
                    list.AddRange(CreateAudit(printers));
                    PrintersCount = printers.Count;

                    var contacts = await ldap.SearchAsync("(&(objectClass=contact)(objectCategory=person))");
                    list.AddRange(CreateAudit(contacts));
                    ContactsCount = contacts.Count;
                }
            });

            AuditList = list;
        }

        private List<AuditItem> CreateAudit(List<LdapEntry> ldapEntries)
        {
            List<AuditItem> list = new List<AuditItem>();

            foreach (LdapEntry entry in ldapEntries)
            {
                if (entry.DirectoryAttributes.Contains("whencreated") && entry.DirectoryAttributes.Contains("whenchanged"))
                {
                    DateTime whencreated = GetDate(entry.DirectoryAttributes["whencreated"].GetValue<string>(), ADAttribute.DateTypes.Date);
                    DateTime whenchanged = GetDate(entry.DirectoryAttributes["whenchanged"].GetValue<string>(), ADAttribute.DateTypes.Date);
                    if (whencreated >= DateTime.Today || whenchanged >= DateTime.Today)
                    {
                        list.Add(new AuditItem()
                        {
                            CN = entry.DirectoryAttributes["CN"].GetValue<string>(),
                            Action = whenchanged > whencreated ? "Changed" : "Created",
                            Date = whenchanged > whencreated ? whenchanged : whencreated,
                            DistinguishedName = entry.DirectoryAttributes["DistinguishedName"].GetValue<string>()
                        });
                    }
                }
            }

            return list;
        }

        private DateTime GetDate(string sDate, ADAttribute.DateTypes dateType)
        {
            if (sDate == "0")
                return new DateTime(1601, 01, 01, 0, 0, 0, DateTimeKind.Utc);

            if (dateType == ADAttribute.DateTypes.Date)
            {
                if (sDate.EndsWith("Z"))
                {
                    int year = Convert.ToInt32(sDate.Substring(0, 4));
                    int month = Convert.ToInt32(sDate.Substring(4, 2));
                    int day = Convert.ToInt32(sDate.Substring(6, 2));

                    int hour = 0;
                    int minute = 0;
                    int second = 0;

                    if (sDate.Length > 8)
                    {
                        hour = Convert.ToInt32(sDate.Substring(8, 2));
                        minute = Convert.ToInt32(sDate.Substring(10, 2));
                        second = Convert.ToInt32(sDate.Substring(12, 2));
                    }
                    return new DateTime(year, month, day, hour, minute, second);
                }
                else
                {
                    return Convert.ToDateTime(sDate);
                }
            }
            else
            {
                if (sDate.Length == 18)
                    return DateTime.FromFileTime(long.Parse(sDate));
                else
                    return new DateTime(1601, 01, 01, 0, 0, 0, DateTimeKind.Utc);
            }
        }

    }

}