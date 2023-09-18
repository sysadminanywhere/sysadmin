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

namespace Sysadmin.ViewModels
{
    public partial class UserViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        private INavigationService _navigationService;
        private IExchangeService _exchangeService;

        [ObservableProperty]
        private UserEntry _user = new UserEntry();

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        public UserViewModel(INavigationService navigationService, IExchangeService exchangeService)
        {
            _navigationService = navigationService;
            _exchangeService = exchangeService;
        }

        public void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();

            if (_exchangeService.GetParameter() is UserEntry entry)
                User = entry;
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
            _navigationService.Navigate(typeof(Views.Pages.UsersPage));
        }

        [RelayCommand]
        private void OnEdit()
        {
            _navigationService.Navigate(typeof(Views.Pages.EditUserPage));
        }

        [RelayCommand]
        private void OnOptions()
        {
            _navigationService.Navigate(typeof(Views.Pages.UserOptionsPage));
        }

        [RelayCommand]
        private void OnResetPassword()
        {
            _navigationService.Navigate(typeof(Views.Pages.ResetPasswordPage));
        }

        [RelayCommand]
        private async Task OnDelete()
        {
            try
            {
                await Delete(User);
                _navigationService.Navigate(typeof(Views.Pages.UsersPage));
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

        public async Task Delete(UserEntry user)
        {
            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    using (var usersRepository = new UsersRepository(ldap))
                    {
                        await usersRepository.DeleteAsync(user);
                    }
                }
            });
        }

        public async Task UpdatePhoto(string distinguishedName, byte[] photo)
        {
            try
            {
                await UpdatePhotoAsync(distinguishedName, photo);
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

        private async Task UpdatePhotoAsync(string distinguishedName, byte[] photo)
        {
            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    var image = new DirectoryModificationAttribute
                    {
                        LdapModOperation = LdapModOperation.LDAP_MOD_REPLACE,
                        Name = "jpegPhoto"
                    };
                    image.Add(photo);
                    await ldap.SendRequestAsync(new ModifyRequest(distinguishedName, image));
                }
            });

        }
    }
}
