using CommunityToolkit.Mvvm.ComponentModel;
using Sysadmin.Services;
using System;
using System.Collections.ObjectModel;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace Sysadmin.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private bool isInitialized = false;

        [ObservableProperty]
        private string _applicationTitle = String.Empty;

        [ObservableProperty]
        private ObservableCollection<object> _navigationItems = new();

        [ObservableProperty]
        private ObservableCollection<object> _navigationFooter = new();

        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = new();

        private IStateService stateService;

        public MainWindowViewModel(INavigationService navigationService, IStateService stateService)
        {
            this.stateService = stateService;

            if (!isInitialized)
                InitializeViewModel();
        }

        public void InitializeViewModel()
        {
            ApplicationTitle = "Sysadmin";

            NavigationItems = new ObservableCollection<object>
            {
                new NavigationViewItem()
                {
                    Content = "Login",
                    Tag = "login",
                    Icon = new SymbolIcon { Symbol = SymbolRegular.PeopleAudience24 },
                    TargetPageType = typeof(Views.Pages.LoginPage),
                    Visibility = stateService.IsLoggedIn ? System.Windows.Visibility.Collapsed: System.Windows.Visibility.Visible
                },
                new NavigationViewItem()
                {
                    Content = "Home",
                    Tag = "dashboard",
                    Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
                    TargetPageType = typeof(Views.Pages.DashboardPage), 
                    Visibility = stateService.IsLoggedIn ? System.Windows.Visibility.Visible: System.Windows.Visibility.Collapsed
                },
                new NavigationViewItem()
                {
                    Content = "Users",
                    Tag = "users",
                    Icon = new SymbolIcon { Symbol = SymbolRegular.Person24 },
                    TargetPageType = typeof(Views.Pages.UsersPage),
                    Visibility = stateService.IsLoggedIn ? System.Windows.Visibility.Visible: System.Windows.Visibility.Collapsed
                },
                new NavigationViewItem()
                {
                    Content = "Computers",
                    Tag = "computers",
                    Icon = new SymbolIcon { Symbol = SymbolRegular.Desktop24 },
                    TargetPageType = typeof(Views.Pages.ComputersPage),
                    Visibility = stateService.IsLoggedIn ? System.Windows.Visibility.Visible: System.Windows.Visibility.Collapsed
                },
                new NavigationViewItem()
                {
                    Content = "Groups",
                    Tag = "groups",
                    Icon = new SymbolIcon { Symbol = SymbolRegular.People24 },
                    TargetPageType = typeof(Views.Pages.GroupsPage),
                    Visibility = stateService.IsLoggedIn ? System.Windows.Visibility.Visible: System.Windows.Visibility.Collapsed
                },
                new NavigationViewItem()
                {
                    Content = "Printers",
                    Tag = "printers",
                    Icon = new SymbolIcon { Symbol = SymbolRegular.Print24 },
                    TargetPageType = typeof(Views.Pages.PrintersPage),
                    Visibility = stateService.IsLoggedIn ? System.Windows.Visibility.Visible: System.Windows.Visibility.Collapsed
                },
                new NavigationViewItem()
                {
                    Content = "Contacts",
                    Tag = "contacts",
                    Icon = new SymbolIcon { Symbol = SymbolRegular.ContactCard24 },
                    TargetPageType = typeof(Views.Pages.ContactsPage),
                    Visibility = stateService.IsLoggedIn ? System.Windows.Visibility.Visible: System.Windows.Visibility.Collapsed
                },
                new NavigationViewItem()
                {
                    Content = "Reports",
                    Tag = "reports",
                    Icon = new SymbolIcon { Symbol = SymbolRegular.ChartMultiple24 },
                    TargetPageType = typeof(Views.Pages.ReportsPage),
                    Visibility = stateService.IsLoggedIn ? System.Windows.Visibility.Visible: System.Windows.Visibility.Collapsed
                }
            };

            NavigationFooter = new ObservableCollection<object>
            {
                new NavigationViewItem()
                {
                    Content = "Settings",
                    Tag = "settings",
                    Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                    TargetPageType = typeof(Views.Pages.SettingsPage)
                }
            };

            TrayMenuItems = new ObservableCollection<MenuItem>
            {
                new MenuItem
                {
                    Header = "Home",
                    Tag = "tray_home"
                }
            };

            isInitialized = true;
        }

    }
}
