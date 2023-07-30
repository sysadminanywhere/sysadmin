using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Sysadmin.Services;
using SysAdmin.ActiveDirectory.Services.Ldap;
using SysAdmin.Services;
using System.Security;
using System.Threading.Tasks;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace Sysadmin.ViewModels
{
    public partial class LoginViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        private INavigationService _navigationService;

        private IStateService stateService;

        private MainWindowViewModel mainWindowViewModel;

        private ISettingsService settingsService;

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
        private string _port = "389";

        [ObservableProperty]
        private bool _ssl;

        [ObservableProperty]
        private bool _showError;

        [ObservableProperty]
        private bool _showConnecting;


        public LoginViewModel(INavigationService navigationService, IStateService stateService, MainWindowViewModel mainWindowViewModel, ISettingsService settingsService)
        {
            _navigationService = navigationService;
            this.stateService = stateService;
            this.mainWindowViewModel = mainWindowViewModel;
            this.settingsService = settingsService;
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
        }

        [RelayCommand]
        private async void OnLogin()
        {

            ShowConnecting = true;

            bool isConnected = false;


            switch (SelectedIndex)
            {
                case 0:
                    App.SERVER = null;
                    if (UseCredentials)
                    {
                        App.CREDENTIAL = new Credential()
                        {
                            UserName = UserName,
                            Password = Password.ToString()
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
                        Port = int.Parse(Port),
                        IsSSL = Ssl
                    };
                    App.CREDENTIAL = new Credential()
                    {
                        UserName = UserName,
                        Password = Password.ToString()
                    };
                    break;
            }


            await Task.Run(() =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    isConnected = ldap.IsConnected;
                }
            });

            if (isConnected)
            {
                settingsService.ServerName = ServerName;
                settingsService.UserName = UserName;
                settingsService.ServerPort = int.Parse(Port);
                settingsService.IsSSL = Ssl;

                stateService.IsLoggedIn = true;
                mainWindowViewModel.InitializeViewModel();
                _navigationService.Navigate(typeof(Views.Pages.DashboardPage));
            }
            else 
            {
                ShowError = true;
            }

        }

    }
}
