using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LdapForNet;
using Sysadmin.Services;
using SysAdmin.ActiveDirectory;
using SysAdmin.ActiveDirectory.Models;
using SysAdmin.ActiveDirectory.Repositories;
using SysAdmin.ActiveDirectory.Services.Ldap;
using System;
using System.Threading.Tasks;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace Sysadmin.ViewModels
{
    public partial class UserOptionsViewModel : ViewModel
    {
        private bool isInitialized = false;

        private INavigationService navigationService;
        private IExchangeService exchangeService;
        private ISnackbarService snackbarService;

        [ObservableProperty]
        private UserEntry _user = new UserEntry();

        [ObservableProperty]
        private bool _isCannotChangePassword;

        [ObservableProperty]
        private bool _isPasswordNeverExpires;

        [ObservableProperty]
        private bool _isAccountDisabled;

        [ObservableProperty]
        private bool _isMustChangePassword;

        public UserOptionsViewModel(INavigationService navigationService, IExchangeService exchangeService, ISnackbarService snackbarService)
        {
            this.navigationService = navigationService;
            this.exchangeService = exchangeService;
            this.snackbarService = snackbarService;
        }

        public override void OnNavigatedTo()
        {
            if (!isInitialized)
                InitializeViewModel();

            if (exchangeService.GetParameter() is UserEntry entry)
            {
                User = entry;

                UserAccountControls userAccountControl = User.UserControl;

                IsCannotChangePassword = (userAccountControl & UserAccountControls.PASSWD_CANT_CHANGE) == UserAccountControls.PASSWD_CANT_CHANGE;
                IsPasswordNeverExpires = (userAccountControl & UserAccountControls.DONT_EXPIRE_PASSWD) == UserAccountControls.DONT_EXPIRE_PASSWD;
                IsAccountDisabled = (userAccountControl & UserAccountControls.ACCOUNTDISABLE) == UserAccountControls.ACCOUNTDISABLE;

                /*
                    pwdLastSet : If the value is set to 0 ...
                    userAccountControl : and the UF_DONT_EXPIRE_PASSWD flag is not set.
                */

                bool isDontExpirePassword = (userAccountControl & UserAccountControls.DONT_EXPIRE_PASSWD) == UserAccountControls.DONT_EXPIRE_PASSWD;
                if (!isDontExpirePassword && User.PasswordLastSet == new DateTime(1601, 01, 01, 0, 0, 0, DateTimeKind.Utc))
                    IsMustChangePassword = true;
                else
                    IsMustChangePassword = false;
            }
        }

        private void InitializeViewModel()
        {
            isInitialized = true;
        }

        [RelayCommand]
        private void OnClose()
        {
            navigationService.Navigate(typeof(Views.Pages.UserPage));
        }


        [RelayCommand]
        private async Task OnOk()
        {
            try
            {
                await ChangeUserOptions(User, IsCannotChangePassword, IsPasswordNeverExpires, IsAccountDisabled, IsMustChangePassword);
                navigationService.Navigate(typeof(Views.Pages.UserPage));
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

        public async Task ChangeUserOptions(UserEntry user, bool isCannotChangePassword, bool isPasswordNeverExpires, bool isAccountDisabled, bool isMustChangePassword)
        {
            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    using (var usersRepository = new UsersRepository(ldap))
                    {
                        await usersRepository.ChangeUserAccountControlAsync(user, isCannotChangePassword, isPasswordNeverExpires, isAccountDisabled);

                        if (isMustChangePassword)
                            await usersRepository.MustChangePasswordAsync(user);
                    }
                }
            });
        }

    }
}
