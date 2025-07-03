using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Sysadmin.Services;
using SysAdmin.ActiveDirectory.Models;
using System.Threading.Tasks;
using System;
using Wpf.Ui;
using LdapForNet;
using SysAdmin.ActiveDirectory.Services.Ldap;
using SysAdmin.ActiveDirectory.Repositories;
using System.Security;

namespace Sysadmin.ViewModels
{
    public partial class ResetPasswordViewModel : ViewModel
    {
        private bool _isInitialized = false;

        private INavigationService _navigationService;
        private IExchangeService _exchangeService;

        [ObservableProperty]
        private UserEntry _user = new UserEntry();

        [ObservableProperty]
        private SecureString _password = new SecureString();

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        public ResetPasswordViewModel(INavigationService navigationService, IExchangeService exchangeService)
        {
            _navigationService = navigationService;
            _exchangeService = exchangeService;
        }

        public override void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();

            if (_exchangeService.GetParameter() is UserEntry entry)
                User = entry;
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
                await ResetPassword(User, new System.Net.NetworkCredential(string.Empty, Password).Password);
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

        public async Task ResetPassword(UserEntry user, string password)
        {
            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    using (var usersRepository = new UsersRepository(ldap))
                    {
                        await usersRepository.ResetPasswordAsync(user, password);
                    }
                }
            });
        }

    }
}
