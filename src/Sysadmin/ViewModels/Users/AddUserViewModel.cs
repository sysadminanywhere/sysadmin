using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LdapForNet;
using SysAdmin.ActiveDirectory;
using SysAdmin.ActiveDirectory.Models;
using SysAdmin.ActiveDirectory.Repositories;
using SysAdmin.ActiveDirectory.Services.Ldap;
using System;
using System.Security;
using System.Threading.Tasks;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace Sysadmin.ViewModels
{
    public partial class AddUserViewModel : ViewModel
    {
        private bool isInitialized = false;

        private INavigationService navigationService;
        private ISnackbarService snackbarService;

        [ObservableProperty]
        private UserEntry _user = new UserEntry();

        [ObservableProperty]
        private string _distinguishedName;

        [ObservableProperty]
        private SecureString _password;

        [ObservableProperty]
        private bool _isCannotChangePassword;

        [ObservableProperty]
        private bool _isPasswordNeverExpires;

        [ObservableProperty]
        private bool _isAccountDisabled;

        [ObservableProperty]
        private bool _isMustChangePassword;


        public AddUserViewModel(INavigationService navigationService, ISnackbarService snackbarService)
        {
            this.navigationService = navigationService;
            this.snackbarService = snackbarService;
        }

        public override void OnNavigatedTo()
        {
            if (!isInitialized)
                InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            isInitialized = true;
        }

        [RelayCommand]
        private void OnClose()
        {
            navigationService.Navigate(typeof(Views.Pages.UsersPage));
        }

        [RelayCommand]
        private async Task OnAdd()
        {
            try
            {
                if (string.IsNullOrEmpty(User.CN))
                    User.CN = User.DisplayName;

                if (string.IsNullOrEmpty(User.Name))
                    User.Name = User.DisplayName;

                await Add(DistinguishedName, User, new System.Net.NetworkCredential(string.Empty, Password).Password, IsCannotChangePassword, IsPasswordNeverExpires, IsAccountDisabled, IsMustChangePassword);
                navigationService.Navigate(typeof(Views.Pages.UsersPage));
            }
            catch (LdapException le)
            {
                snackbarService.Show("Error",
                    LdapResult.GetErrorMessageFromResult(le.ResultCode),
                    ControlAppearance.Secondary,
                    new SymbolIcon(SymbolRegular.ErrorCircle12),
                    TimeSpan.FromSeconds(5)
                );
            }
            catch (Exception ex)
            {
                snackbarService.Show("Error",
                    ex.Message,
                    ControlAppearance.Secondary,
                    new SymbolIcon(SymbolRegular.ErrorCircle12),
                    TimeSpan.FromSeconds(5)
                );
            }
        }

        public async Task Add(string distinguishedName, UserEntry user, string password, bool isCannotChangePassword, bool isPasswordNeverExpires, bool isAccountDisabled, bool isMustChangePassword)
        {
            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    using (var usersRepository = new UsersRepository(ldap))
                    {
                        await usersRepository.AddAsync(distinguishedName, user, password, isCannotChangePassword, isPasswordNeverExpires, isAccountDisabled, isMustChangePassword);
                    }
                }
            });
        }

    }
}
