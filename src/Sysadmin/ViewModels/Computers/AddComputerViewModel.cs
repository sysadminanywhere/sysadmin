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
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace Sysadmin.ViewModels
{
    public partial class AddComputerViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        private INavigationService _navigationService;

        [ObservableProperty]
        private ComputerEntry _computer = new ComputerEntry();

        [ObservableProperty]
        private string _distinguishedName;

        [ObservableProperty]
        private bool _isAccountEnabled = true;

        [ObservableProperty]
        private string _errorMessage;

        public AddComputerViewModel(INavigationService navigationService)
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
            _navigationService.Navigate(typeof(Views.Pages.ComputersPage));
        }

        [RelayCommand]
        private async Task OnAdd()
        {
            try
            {
                await Add(DistinguishedName, Computer, IsAccountEnabled);
                _navigationService.Navigate(typeof(Views.Pages.ComputersPage));
            }
            catch (LdapException le)
            {
                ErrorMessage = LdapResult.GetErrorMessageFromResult(le.ResultCode);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

        }

        public async Task Add(string distinguishedName, ComputerEntry computer, bool isEnabled)
        {
            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    using (var computersRepository = new ComputersRepository(ldap))
                    {
                        await computersRepository.AddAsync(distinguishedName, computer, isEnabled);
                    }
                }
            });
        }

    }

}