using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Sysadmin.Services;
using System;
using System.Collections.ObjectModel;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace Sysadmin.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private bool _isInitialized = false;

        [ObservableProperty]
        private string _applicationTitle = String.Empty;

        [ObservableProperty]
        private ObservableCollection<INavigationControl> _navigationItems = new();

        [ObservableProperty]
        private ObservableCollection<INavigationControl> _navigationFooter = new();

        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = new();

        private IStateService stateService;

        public MainWindowViewModel(INavigationService navigationService, IStateService stateService)
        {
            this.stateService = stateService;

            if (!_isInitialized)
                InitializeViewModel();
        }

        public void InitializeViewModel()
        {
            ApplicationTitle = "Sysadmin";

            NavigationItems = new ObservableCollection<INavigationControl>
            {
                new NavigationItem()
                {
                    Content = "Login",
                    PageTag = "login",
                    Icon = SymbolRegular.PeopleAudience24,
                    PageType = typeof(Views.Pages.LoginPage),
                    Visibility = stateService.IsLoggedIn ? System.Windows.Visibility.Collapsed: System.Windows.Visibility.Visible
                },
                new NavigationItem()
                {
                    Content = "Home",
                    PageTag = "dashboard",
                    Icon = SymbolRegular.Home24,
                    PageType = typeof(Views.Pages.DashboardPage), 
                    Visibility = stateService.IsLoggedIn ? System.Windows.Visibility.Visible: System.Windows.Visibility.Collapsed
                },
                new NavigationItem()
                {
                    Content = "Users",
                    PageTag = "users",
                    Icon = SymbolRegular.Person24,
                    PageType = typeof(Views.Pages.UsersPage),
                    Visibility = stateService.IsLoggedIn ? System.Windows.Visibility.Visible: System.Windows.Visibility.Collapsed
                },
                new NavigationItem()
                {
                    Content = "Computers",
                    PageTag = "computers",
                    Icon = SymbolRegular.Desktop24,
                    PageType = typeof(Views.Pages.ComputersPage),
                    Visibility = stateService.IsLoggedIn ? System.Windows.Visibility.Visible: System.Windows.Visibility.Collapsed
                },
                new NavigationItem()
                {
                    Content = "Groups",
                    PageTag = "groups",
                    Icon = SymbolRegular.People24,
                    PageType = typeof(Views.Pages.GroupsPage),
                    Visibility = stateService.IsLoggedIn ? System.Windows.Visibility.Visible: System.Windows.Visibility.Collapsed
                },
                new NavigationItem()
                {
                    Content = "Printers",
                    PageTag = "printers",
                    Icon = SymbolRegular.Print24,
                    PageType = typeof(Views.Pages.PrintersPage),
                    Visibility = stateService.IsLoggedIn ? System.Windows.Visibility.Visible: System.Windows.Visibility.Collapsed
                },
                new NavigationItem()
                {
                    Content = "Contacts",
                    PageTag = "contacts",
                    Icon = SymbolRegular.ContactCard24,
                    PageType = typeof(Views.Pages.ContactsPage),
                    Visibility = stateService.IsLoggedIn ? System.Windows.Visibility.Visible: System.Windows.Visibility.Collapsed
                },
                new NavigationItem()
                {
                    Content = "Reports",
                    PageTag = "reports",
                    Icon = SymbolRegular.ChartMultiple24,
                    PageType = typeof(Views.Pages.ReportsPage),
                    Visibility = stateService.IsLoggedIn ? System.Windows.Visibility.Visible: System.Windows.Visibility.Collapsed
                }
            };

            NavigationFooter = new ObservableCollection<INavigationControl>
            {
                new NavigationItem()
                {
                    Content = "Settings",
                    PageTag = "settings",
                    Icon = SymbolRegular.Settings24,
                    PageType = typeof(Views.Pages.SettingsPage)
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

            _isInitialized = true;
        }

    }
}
