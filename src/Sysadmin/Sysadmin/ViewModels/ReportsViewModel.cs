using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using SysAdmin.Services;
using SysAdmin.Services.Reports;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

namespace SysAdmin.ViewModels
{
    public class ReportsViewModel : ObservableObject
    {

        public ObservableCollection<GroupReportList> GroupedReports { get; private set; } = new ObservableCollection<GroupReportList>();
        private List<IReport> reports;

        INavigationService navigation = App.Current.Services.GetService<INavigationService>();
        INotificationService notification = App.Current.Services.GetService<INotificationService>();
        IBusyService busyService = App.Current.Services.GetService<IBusyService>();

        public ReportsViewModel()
        {

        }

        public void ListAsync()
        {
            reports = new List<IReport>();

            reports.Add(new ReportFromSearch("Computers", "Computers", "All computers", "(objectClass=computer)", new Dictionary<string, string>() { { "cn", "Name" }, { "operatingsystem", "Operating system" }, { "operatingsystemservicepack", "Service pack" } }));
            reports.Add(new ReportFromSearch("Computers", "Controllers", "All controllers", "(&(objectCategory=computer)(primaryGroupID=516))", new Dictionary<string, string>() { { "cn", "Name" }, { "operatingsystem", "Operating system" }, { "operatingsystemservicepack", "Service pack" } }));
            reports.Add(new ReportFromSearch("Computers", "Disabled", "Disabled computers", "(&(objectCategory=computer)(userAccountControl:1.2.840.113556.1.4.803:=2))", new Dictionary<string, string>() { { "cn", "Name" }, { "operatingsystem", "Operating system" }, { "operatingsystemservicepack", "Service pack" } }));
            reports.Add(new ReportFromSearch("Computers", "Servers", "All servers", "(&(objectCategory=computer)(operatingSystem=Windows*Server*))", new Dictionary<string, string>() { { "cn", "Name" }, { "operatingsystem", "Operating system" }, { "operatingsystemservicepack", "Service pack" } }));
            reports.Add(new ReportFromSearch("Computers", "Created", "Created dates", "(objectClass=computer)", new Dictionary<string, string>() { { "cn", "Name" }, { "operatingsystem", "Operating system" }, { "whencreated", "When created" } }));
            reports.Add(new ReportFromSearch("Computers", "Last logon", "Computers with last logon dates", "(objectClass=computer)", new Dictionary<string, string>() { { "cn", "Name" }, { "operatingsystem", "Operating system" }, { "lastlogontimestamp", "Last logon" } }));

            reports.Add(new ReportFromSearch("Groups", "Groups", "All groups", "(objectClass=group)", new Dictionary<string, string>() { { "cn", "Name" }, { "description", "Description" } }));
            reports.Add(new ReportFromSearch("Groups", "Domain security", "Domain security groups", "(&(objectCategory=group)(groupType:1.2.840.113556.1.4.803:=2147483652))", new Dictionary<string, string>() { { "cn", "Name" }, { "description", "Description" } }));
            reports.Add(new ReportFromSearch("Groups", "Global distribution", "Global distribution groups", "(&(objectCategory=group)(groupType:1.2.840.113556.1.4.803:=2)(!(groupType:1.2.840.113556.1.4.803:=2147483648)))", new Dictionary<string, string>() { { "cn", "Name" }, { "description", "Description" } }));
            reports.Add(new ReportFromSearch("Groups", "Global security", "Global security groups", "(&(objectCategory=group)(groupType:1.2.840.113556.1.4.803:=2147483650))", new Dictionary<string, string>() { { "cn", "Name" }, { "description", "Description" } }));
            reports.Add(new ReportFromSearch("Groups", "Universal security", "Universal security groups", "(&(objectCategory=group)(groupType:1.2.840.113556.1.4.803:=2147483656))", new Dictionary<string, string>() { { "cn", "Name" }, { "description", "Description" } }));
            reports.Add(new ReportFromSearch("Groups", "Created", "Created dates", "(objectClass=group)", new Dictionary<string, string>() { { "cn", "Name" }, { "description", "Description" }, { "whencreated", "When created" } }));

            reports.Add(new ReportFromSearch("Users", "Users", "All users", "(&(objectClass=user)(objectCategory=person))", new Dictionary<string, string>() { { "cn", "Name" }, { "description", "Description" } }));
            reports.Add(new ReportFromSearch("Users", "Change password at next logon", "Users must change password at next logon", "(&(objectCategory=User)(pwdLastSet=0))", new Dictionary<string, string>() { { "cn", "Name" }, { "description", "Description" } }));
            reports.Add(new ReportFromSearch("Users", "Disabled", "Disabled users", "(&(objectCategory=user)(userAccountControl:1.2.840.113556.1.4.803:=2))", new Dictionary<string, string>() { { "cn", "Name" }, { "description", "Description" } }));
            reports.Add(new ReportFromSearch("Users", "Locked", "Locked out users", "(&(objectCategory=user)(userAccountControl:1.2.840.113556.1.4.803:=10))", new Dictionary<string, string>() { { "cn", "Name" }, { "description", "Description" } }));
            reports.Add(new ReportFromSearch("Users", "Password never expires", "Password never expires users", "(&(objectCategory=User)(userAccountControl:1.2.840.113556.1.4.803:=65536))", new Dictionary<string, string>() { { "cn", "Name" }, { "description", "Description" } }));
            reports.Add(new ReportFromSearch("Users", "Created", "Created dates", "(&(objectClass=user)(objectCategory=person))", new Dictionary<string, string>() { { "cn", "Name" }, { "description", "Description" }, { "whencreated", "When created" } }));
            reports.Add(new ReportFromSearch("Users", "Logon scripts", "Logon scripts for users", "(&(objectClass=user)(objectCategory=person))", new Dictionary<string, string>() { { "cn", "Name" }, { "description", "Description" }, { "scriptpath", "Script path" } }));
            reports.Add(new ReportFromSearch("Users", "Profile paths", "Profile paths for users", "(&(objectClass=user)(objectCategory=person))", new Dictionary<string, string>() { { "cn", "Name" }, { "description", "Description" }, { "profilepath", "Profile path" } }));
            reports.Add(new ReportFromSearch("Users", "Home folders", "Home folders for users", "(&(objectClass=user)(objectCategory=person))", new Dictionary<string, string>() { { "cn", "Name" }, { "description", "Description" }, { "homedirectory", "Home directory" } }));
            reports.Add(new ReportFromSearch("Users", "Last logon", "Users with last logon dates", "(&(objectClass=user)(objectCategory=person))", new Dictionary<string, string>() { { "cn", "Name" }, { "description", "Description" }, { "lastlogon", "Last logon" } }));

            reports.Add(new ReportFromSearch("Others", "Printers", "All printers", "(objectClass=printQueue)", new Dictionary<string, string>() { { "cn", "Name" }, { "description", "Description" } }));
            //reports.Add(new ReportFromSearch("Others", "Non mail-enabled objects", "Objects without primary e-mail address", "(&(objectClass=*)(cn=*)(!mail=*))", new Dictionary<string, string>() { { "cn", "Name" }, { "description", "Description" } }));

            var query = from item in reports
                        group item by item.Group into g
                        orderby g.Key
                        select new GroupReportList(g) { Key = g.Key };

            GroupedReports = new ObservableCollection<GroupReportList>(query);

            OnPropertyChanged(nameof(GroupedReports));
        }

    }
}