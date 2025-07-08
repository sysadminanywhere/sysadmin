using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Sysadmin.Services;
using SysAdmin.ActiveDirectory.Services.Ldap;
using SysAdmin.Services;
using System;
using System.Security;
using System.Threading.Tasks;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace Sysadmin.ViewModels
{
    public partial class LoginViewModel : ViewModel
    {
        private bool isInitialized = false;

        private INavigationService navigationService;
        private IStateService stateService;
        private MainWindowViewModel mainWindowViewModel;
        private ISettingsService settingsService;
        private ISnackbarService snackbarService;

        [ObservableProperty]
        private int _selectedIndex;

        [ObservableProperty]
        private string _userName;

        [ObservableProperty]
        private SecureString _password;

        [ObservableProperty]
        private bool _useCredentials;

        [ObservableProperty]
        private string _serverName;

        [ObservableProperty]
        private int _port = 389;

        [ObservableProperty]
        private bool _ssl;

        public LoginViewModel(INavigationService navigationService, 
            IStateService stateService, 
            MainWindowViewModel mainWindowViewModel, 
            ISettingsService settingsService, 
            ISnackbarService snackbarService)
        {
            this.navigationService = navigationService;
            this.stateService = stateService;
            this.mainWindowViewModel = mainWindowViewModel;
            this.settingsService = settingsService;
            this.snackbarService = snackbarService;
        }

        public override void OnNavigatedTo()
        {
            if (!isInitialized)
                InitializeViewModel();

            SelectedIndex = settingsService.LoginSelectedIndex;
            UseCredentials = settingsService.LoginUseCredentials;

            ServerName = settingsService.ServerName;
            UserName = settingsService.UserName;
            Port = settingsService.ServerPort;
            Ssl = settingsService.IsSSL;
        }

        private void InitializeViewModel()
        {
            isInitialized = true;
        }

        [RelayCommand]
        private async void OnLogin()
        {

            bool isConnected = false;
            string message = string.Empty;

            switch (SelectedIndex)
            {
                case 0:
                    App.SERVER = null;
                    if (UseCredentials)
                    {
                        App.CREDENTIAL = new Credential()
                        {
                            UserName = UserName,
                            Password = new System.Net.NetworkCredential(string.Empty, Password).Password
                    };
                    }
                    else
                    {
                        App.CREDENTIAL = null;
                    }
                    break;

                case 1:
                    App.SERVER = new SecureServer()
                    {
                        ServerName = ServerName,
                        Port = Port,
                        IsSSL = Ssl
                    };
                    App.CREDENTIAL = new Credential()
                    {
                        UserName = UserName,
                        Password = new System.Net.NetworkCredential(string.Empty, Password).Password
                    };
                    break;
            }


            await Task.Run(() =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    isConnected = ldap.IsConnected;
                    message = ldap.ErrorMessage;

                    if(isConnected)
                        App.CONTAINERS = new SysAdmin.ActiveDirectory.ADContainers(ldap);
                }
            });

            if (isConnected)
            {
                settingsService.LoginSelectedIndex = SelectedIndex;
                settingsService.LoginUseCredentials = UseCredentials;

                settingsService.ServerName = ServerName;
                settingsService.UserName = UserName;
                settingsService.ServerPort = Port;
                settingsService.IsSSL = Ssl;

                stateService.IsLoggedIn = true;
                mainWindowViewModel.InitializeViewModel();

                navigationService.GetNavigationControl().ClearJournal();
                navigationService.Navigate(typeof(Views.Pages.DashboardPage));
            }
            else
            {
                snackbarService.Show("Error",
                    message,
                    ControlAppearance.Danger,
                    new SymbolIcon(SymbolRegular.ErrorCircle12),
                    TimeSpan.FromSeconds(5)
                );
            }

        }

    }
}
