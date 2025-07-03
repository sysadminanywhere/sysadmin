using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Sysadmin.Services;
using SysAdmin.ActiveDirectory.Models;
using System.Threading.Tasks;
using System;
using LdapForNet;
using SysAdmin.ActiveDirectory.Services.Ldap;
using SysAdmin.ActiveDirectory.Repositories;
using Wpf.Ui;

namespace Sysadmin.ViewModels
{
    public partial class PrinterViewModel : ViewModel
    {
        private bool _isInitialized = false;

        private INavigationService _navigationService;
        private IExchangeService _exchangeService;

        [ObservableProperty]
        private PrinterEntry _printer;

        [ObservableProperty]
        private string _errorMessage;

        public PrinterViewModel(INavigationService navigationService, IExchangeService exchangeService)
        {
            _navigationService = navigationService;
            _exchangeService = exchangeService;
        }

        public override void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();

            if (_exchangeService.GetParameter() is PrinterEntry entry)
                Printer = entry;
        }

        private void InitializeViewModel()
        {
            _isInitialized = true;
        }

        [RelayCommand]
        private void OnClose()
        {
            _navigationService.Navigate(typeof(Views.Pages.PrintersPage));
        }

        [RelayCommand]
        private async Task OnDelete()
        {
            try
            {
                await Delete(Printer);
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

        public async Task Delete(PrinterEntry printer)
        {
            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    using (var printersRepository = new PrintersRepository(ldap))
                    {
                        await printersRepository.DeleteAsync(printer);
                    }
                }
            });
        }

    }
}
