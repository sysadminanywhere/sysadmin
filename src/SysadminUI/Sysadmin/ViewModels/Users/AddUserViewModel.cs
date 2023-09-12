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

namespace Sysadmin.ViewModels
{
    public partial class AddUserViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        private INavigationService _navigationService;

        [ObservableProperty]
        private UserEntry _user = new UserEntry();

        [ObservableProperty]
        private string _distinguishedName;

        [ObservableProperty]
        private SecureString _password;

        [ObservableProperty]
        private string _errorMessage;

        public AddUserViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
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
        private void OnClose()
        {
            _navigationService.Navigate(typeof(Views.Pages.UsersPage));
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

                await Add(DistinguishedName, User, new System.Net.NetworkCredential(string.Empty, Password).Password);
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

        public async Task Add(string distinguishedName, UserEntry user, string password)
        {
            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    using (var usersRepository = new UsersRepository(ldap))
                    {
                        await usersRepository.AddAsync(distinguishedName, user, password);
                    }
                }
            });
        }

    }
}
