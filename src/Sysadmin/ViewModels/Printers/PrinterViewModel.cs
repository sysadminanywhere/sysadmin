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
    public partial class PrinterViewModel : ViewModel
    {
        private bool isInitialized = false;

        private INavigationService navigationService;
        private IExchangeService exchangeService;
        private ISnackbarService snackbarService;

        [ObservableProperty]
        private PrinterEntry _printer;

        public PrinterViewModel(INavigationService navigationService, IExchangeService exchangeService, ISnackbarService snackbarService)
        {
            this.navigationService = navigationService;
            this.exchangeService = exchangeService;
            this.snackbarService = snackbarService;
        }

        public override void OnNavigatedTo()
        {
            if (!isInitialized)
                InitializeViewModel();

            if (exchangeService.GetParameter() is PrinterEntry entry)
                Printer = entry;
        }

        private void InitializeViewModel()
        {
            isInitialized = true;
        }

        [RelayCommand]
        private async Task OnDelete()
        {
            try
            {
                await Delete(Printer);
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
