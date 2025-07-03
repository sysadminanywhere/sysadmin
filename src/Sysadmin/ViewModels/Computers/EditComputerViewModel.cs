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

namespace Sysadmin.ViewModels
{
    public partial class EditComputerViewModel : ViewModel
    {
        private bool _isInitialized = false;

        private INavigationService _navigationService;
        private IExchangeService _exchangeService;

        [ObservableProperty]
        private ComputerEntry _computer = new ComputerEntry();

        [ObservableProperty]
        private string _distinguishedName = string.Empty;

        [ObservableProperty]
        private bool _isAccountEnabled = true;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        public EditComputerViewModel(INavigationService navigationService, IExchangeService exchangeService)
        {
            _navigationService = navigationService;
            _exchangeService = exchangeService;
        }

        public override void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();

            if (_exchangeService.GetParameter() is ComputerEntry entry)
                Computer = entry;
        }

        private void InitializeViewModel()
        {
            _isInitialized = true;
        }

        [RelayCommand]
        private void OnClose()
        {
            _navigationService.Navigate(typeof(Views.Pages.ComputerPage));
        }

        [RelayCommand]
        private async Task OnEdit()
        {
            try
            {
                await Edit(Computer);
                _navigationService.Navigate(typeof(Views.Pages.ComputerPage));
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

        public async Task Edit(ComputerEntry computer)
        {
            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    using (var computersRepository = new ComputersRepository(ldap))
                    {
                        await computersRepository.ModifyAsync(computer);
                    }
                }
            });
        }

    }

}