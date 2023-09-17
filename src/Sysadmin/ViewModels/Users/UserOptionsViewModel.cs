using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Sysadmin.Services;
using SysAdmin.ActiveDirectory.Models;
using System.Threading.Tasks;
using System;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using LdapForNet;
using SysAdmin.ActiveDirectory.Services.Ldap;
using SysAdmin.ActiveDirectory.Repositories;
using System.Security;
using System.Text.RegularExpressions;
using Wpf.Ui.Controls.Interfaces;
using static LdapForNet.Native.Native;
using SysAdmin.ActiveDirectory;

namespace Sysadmin.ViewModels
{
    public partial class UserOptionsViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        private INavigationService _navigationService;
        private IExchangeService _exchangeService;

        [ObservableProperty]
        private UserEntry _user = new UserEntry();

        [ObservableProperty]
        private string _errorMessage;

        [ObservableProperty]
        private bool _isCannotChangePassword;

        [ObservableProperty]
        private bool _isPasswordNeverExpires;

        [ObservableProperty]
        private bool _isAccountDisabled;

        [ObservableProperty]
        private bool _isMustChangePassword;

        public UserOptionsViewModel(INavigationService navigationService, IExchangeService exchangeService)
        {
            _navigationService = navigationService;
            _exchangeService = exchangeService;
        }

        public async void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();

            if (_exchangeService.GetParameter() is UserEntry entry)
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
                if (isDontExpirePassword == false && User.PasswordLastSet == new DateTime(1601, 01, 01, 0, 0, 0, DateTimeKind.Utc))
                    IsMustChangePassword = true;
                else
                    IsMustChangePassword = false;
            }
        }

        public void OnNavigatedFrom()
        {

        }

        private void InitializeViewModel()
        {
            _isInitialized = true;
        }

        [RelayCommand]
        private void OnClose()
        {
            _navigationService.Navigate(typeof(Views.Pages.UserPage));
        }


        [RelayCommand]
        private async Task OnOk()
        {
            try
            {
                await ChangeUserOptions(User, IsCannotChangePassword, IsPasswordNeverExpires, IsAccountDisabled, IsMustChangePassword);
                _navigationService.Navigate(typeof(Views.Pages.UserPage));
            }
            catch (LdapException le)
            {
                ErrorMessage = SysAdmin.ActiveDirectory.LdapResult.GetErrorMessageFromResult(le.ResultCode);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
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
