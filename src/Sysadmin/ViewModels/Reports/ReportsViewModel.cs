using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Sysadmin.Models;
using Sysadmin.Services;
using Sysadmin.Services.Reports;
using SysAdmin.ActiveDirectory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Controls;
using Wpf.Ui.Mvvm.Contracts;


namespace Sysadmin.ViewModels
{
    public partial class ReportsViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        private INavigationService _navigationService;
        private IExchangeService _exchangeService;

        [ObservableProperty]
        private List<IReport> _reports = new List<IReport>();

        public ReportsViewModel(INavigationService navigationService, IExchangeService exchangeService)
        {
            _navigationService = navigationService;
            _exchangeService = exchangeService;
        }

        public void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        public void OnNavigatedFrom()
        {
        }

        private void InitializeViewModel()
        {
            _isInitialized = true;

            ListAsync();
        }

        private void ListAsync()
        {
            Reports = new List<IReport>();

            Reports.Add(new ReportFromSearch("Computers", "Computers", "All computers", "(objectClass=computer)", new Dictionary<string, string>() { { "cn", "Name" }, { "operatingsystem", "Operating system" }, { "operatingsystemservicepack", "Service pack" } }));
            Reports.Add(new ReportFromSearch("Computers", "Controllers", "All controllers", "(&(objectCategory=computer)(primaryGroupID=516))", new Dictionary<string, string>() { { "cn", "Name" }, { "operatingsystem", "Operating system" }, { "operatingsystemservicepack", "Service pack" } }));
            Reports.Add(new ReportFromSearch("Computers", "Disabled", "Disabled computers", "(&(objectCategory=computer)(userAccountControl:1.2.840.113556.1.4.803:=2))", new Dictionary<string, string>() { { "cn", "Name" }, { "operatingsystem", "Operating system" }, { "operatingsystemservicepack", "Service pack" } }));
            Reports.Add(new ReportFromSearch("Computers", "Servers", "All servers", "(&(objectCategory=computer)(operatingSystem=Windows*Server*))", new Dictionary<string, string>() { { "cn", "Name" }, { "operatingsystem", "Operating system" }, { "operatingsystemservicepack", "Service pack" } }));
            Reports.Add(new ReportFromSearch("Computers", "Created", "Created dates", "(objectClass=computer)", new Dictionary<string, string>() { { "cn", "Name" }, { "operatingsystem", "Operating system" }, { "whencreated", "When created" } }));
            Reports.Add(new ReportFromSearch("Computers", "Last logon", "Computers with last logon dates", "(objectClass=computer)", new Dictionary<string, string>() { { "cn", "Name" }, { "operatingsystem", "Operating system" }, { "lastlogontimestamp", "Last logon" } }));

            Reports.Add(new ReportFromSearch("Groups", "Groups", "All groups", "(objectClass=group)", new Dictionary<string, string>() { { "cn", "Name" }, { "description", "Description" } }));
            Reports.Add(new ReportFromSearch("Groups", "Domain security", "Domain security groups", "(&(objectCategory=group)(groupType:1.2.840.113556.1.4.803:=2147483652))", new Dictionary<string, string>() { { "cn", "Name" }, { "description", "Description" } }));
            Reports.Add(new ReportFromSearch("Groups", "Global distribution", "Global distribution groups", "(&(objectCategory=group)(groupType:1.2.840.113556.1.4.803:=2)(!(groupType:1.2.840.113556.1.4.803:=2147483648)))", new Dictionary<string, string>() { { "cn", "Name" }, { "description", "Description" } }));
            Reports.Add(new ReportFromSearch("Groups", "Global security", "Global security groups", "(&(objectCategory=group)(groupType:1.2.840.113556.1.4.803:=2147483650))", new Dictionary<string, string>() { { "cn", "Name" }, { "description", "Description" } }));
            Reports.Add(new ReportFromSearch("Groups", "Universal security", "Universal security groups", "(&(objectCategory=group)(groupType:1.2.840.113556.1.4.803:=2147483656))", new Dictionary<string, string>() { { "cn", "Name" }, { "description", "Description" } }));
            Reports.Add(new ReportFromSearch("Groups", "Created", "Created dates", "(objectClass=group)", new Dictionary<string, string>() { { "cn", "Name" }, { "description", "Description" }, { "whencreated", "When created" } }));

            Reports.Add(new ReportFromSearch("Users", "Users", "All users", "(&(objectClass=user)(objectCategory=person))", new Dictionary<string, string>() { { "cn", "Name" }, { "description", "Description" } }));
            Reports.Add(new ReportFromSearch("Users", "Change password at next logon", "Users must change password at next logon", "(&(objectCategory=User)(pwdLastSet=0))", new Dictionary<string, string>() { { "cn", "Name" }, { "description", "Description" } }));
            Reports.Add(new ReportFromSearch("Users", "Disabled", "Disabled users", "(&(objectCategory=user)(userAccountControl:1.2.840.113556.1.4.803:=2))", new Dictionary<string, string>() { { "cn", "Name" }, { "description", "Description" } }));
            Reports.Add(new ReportFromSearch("Users", "Locked", "Locked out users", "(&(objectCategory=user)(userAccountControl:1.2.840.113556.1.4.803:=10))", new Dictionary<string, string>() { { "cn", "Name" }, { "description", "Description" } }));
            Reports.Add(new ReportFromSearch("Users", "Password never expires", "Password never expires users", "(&(objectCategory=User)(userAccountControl:1.2.840.113556.1.4.803:=65536))", new Dictionary<string, string>() { { "cn", "Name" }, { "description", "Description" } }));
            Reports.Add(new ReportFromSearch("Users", "Created", "Created dates", "(&(objectClass=user)(objectCategory=person))", new Dictionary<string, string>() { { "cn", "Name" }, { "description", "Description" }, { "whencreated", "When created" } }));
            Reports.Add(new ReportFromSearch("Users", "Logon scripts", "Logon scripts for users", "(&(objectClass=user)(objectCategory=person))", new Dictionary<string, string>() { { "cn", "Name" }, { "description", "Description" }, { "scriptpath", "Script path" } }));
            Reports.Add(new ReportFromSearch("Users", "Profile paths", "Profile paths for users", "(&(objectClass=user)(objectCategory=person))", new Dictionary<string, string>() { { "cn", "Name" }, { "description", "Description" }, { "profilepath", "Profile path" } }));
            Reports.Add(new ReportFromSearch("Users", "Home folders", "Home folders for users", "(&(objectClass=user)(objectCategory=person))", new Dictionary<string, string>() { { "cn", "Name" }, { "description", "Description" }, { "homedirectory", "Home directory" } }));
            Reports.Add(new ReportFromSearch("Users", "Last logon", "Users with last logon dates", "(&(objectClass=user)(objectCategory=person))", new Dictionary<string, string>() { { "cn", "Name" }, { "description", "Description" }, { "lastlogon", "Last logon" } }));
            Reports.Add(new ReportFromSearch("Users", "Without manager", "Users without managers", "(&(objectClass=user)(objectCategory=person)(!manager=*))", new Dictionary<string, string>() { { "cn", "Name" }, { "description", "Description" }, { "lastlogon", "Last logon" } }));

            Reports.Add(new ReportFromSearch("Others", "Printers", "All printers", "(objectClass=printQueue)", new Dictionary<string, string>() { { "cn", "Name" }, { "description", "Description" } }));
        }

        [RelayCommand]
        private void OnSelectedItemsChanged(IEnumerable<object> items)
        {
            if (items.Any())
            {
                _exchangeService.SetParameter((IReport)items.First());
                _navigationService.Navigate(typeof(Views.Pages.ReportPage));
            }
        }

    }
}
