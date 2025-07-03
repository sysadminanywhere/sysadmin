using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LdapForNet;
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
    public partial class AddComputerViewModel : ViewModel
    {
        private bool isInitialized = false;

        private INavigationService navigationService;
        private ISnackbarService snackbarService;

        [ObservableProperty]
        private ComputerEntry _computer = new ComputerEntry();

        [ObservableProperty]
        private string _distinguishedName;

        [ObservableProperty]
        private bool _isAccountEnabled = true;

        public AddComputerViewModel(INavigationService navigationService, ISnackbarService snackbarService)
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
            navigationService.Navigate(typeof(Views.Pages.ComputersPage));
        }

        [RelayCommand]
        private async Task OnAdd()
        {
            try
            {
                await Add(DistinguishedName, Computer, IsAccountEnabled);
                navigationService.Navigate(typeof(Views.Pages.ComputersPage));
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