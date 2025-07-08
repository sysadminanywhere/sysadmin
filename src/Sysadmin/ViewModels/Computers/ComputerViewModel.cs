using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LdapForNet;
using Sysadmin.Services;
using Sysadmin.WMI.Services;
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
    public partial class ComputerViewModel : ViewModel
    {
        private bool isInitialized = false;

        private INavigationService navigationService;
        private IExchangeService exchangeService;
        private ISnackbarService snackbarService;

        [ObservableProperty]
        private ComputerEntry _computer = new ComputerEntry();

        public ComputerViewModel(INavigationService navigationService, IExchangeService exchangeService, ISnackbarService snackbarService)
        {
            this.navigationService = navigationService;
            this.exchangeService = exchangeService;
            this.snackbarService = snackbarService;
        }

        public override void OnNavigatedTo()
        {
            if (!isInitialized)
                InitializeViewModel();

            if (exchangeService.GetParameter() is ComputerEntry entry)
                Computer = entry;
        }

        private void InitializeViewModel()
        {
            isInitialized = true;
        }

        [RelayCommand]
        private void OnEdit()
        {
            navigationService.Navigate(typeof(Views.Pages.EditComputerPage));
        }

        [RelayCommand]
        private void OnEvents()
        {
            exchangeService.SetParameter(Computer);
            navigationService.Navigate(typeof(Views.Pages.EventsPage));
        }

        [RelayCommand]
        private void OnServices()
        {
            exchangeService.SetParameter(Computer);
            navigationService.Navigate(typeof(Views.Pages.ServicesPage));
        }

        [RelayCommand]
        private void OnProcesses()
        {
            exchangeService.SetParameter(Computer);
            navigationService.Navigate(typeof(Views.Pages.ProcessesPage));
        }

        [RelayCommand]
        private void OnSoftware()
        {
            exchangeService.SetParameter(Computer);
            navigationService.Navigate(typeof(Views.Pages.SoftwarePage));
        }

        [RelayCommand]
        private void OnHardware()
        {
            exchangeService.SetParameter(Computer);
            navigationService.Navigate(typeof(Views.Pages.HardwarePage));
        }

        [RelayCommand]
        private void OnPerformance()
        {
            exchangeService.SetParameter(Computer);
            navigationService.Navigate(typeof(Views.Pages.PerformancePage));
        }

        [RelayCommand]
        private async Task OnDelete()
        {
            try
            {
                await Delete(Computer);
                navigationService.Navigate(typeof(Views.Pages.ComputersPage));
            }
            catch (LdapException le)
            {
                snackbarService.Show("Error",
                    LdapResult.GetErrorMessageFromResult(le.ResultCode),
                    ControlAppearance.Danger,
                    new SymbolIcon(SymbolRegular.ErrorCircle12),
                    TimeSpan.FromSeconds(5)
                );
            }
            catch (Exception ex)
            {
                snackbarService.Show("Error",
                    ex.Message,
                    ControlAppearance.Danger,
                    new SymbolIcon(SymbolRegular.ErrorCircle12),
                    TimeSpan.FromSeconds(5)
                );
            }
        }

        public async Task Delete(ComputerEntry computer)
        {
            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    using (var computersRepository = new ComputersRepository(ldap))
                    {
                        await computersRepository.DeleteAsync(computer);
                    }
                }
            });
        }

        [RelayCommand]
        private async Task OnReboot()
        {
            await Reboot();
        }

        [RelayCommand]
        private async Task OnShutdown()
        {
            await Shutdown();
        }

        public async Task Reboot()
        {
            WMI.Services.ICredential? credential = null;

            if (App.CREDENTIAL != null)
                credential = new WMI.Services.Credential() { UserName = App.CREDENTIAL.UserName, Password = App.CREDENTIAL.Password };

            try
            {
                await Task.Run(() =>
                {
                    using (var wmi = new WMIService(Computer.DnsHostName, credential, App.SERVER == null ? true : false))
                    {
                        wmi.Invoke("SELECT * FROM Win32_OperatingSystem", "Reboot");
                    }
                });

                snackbarService.Show("Command",
                    "Reboot command sent successfully",
                    ControlAppearance.Success,
                    new SymbolIcon(SymbolRegular.CheckmarkCircle12),
                    TimeSpan.FromSeconds(5)
                );
            }
            catch (Exception ex)
            {
                snackbarService.Show("Error",
                    ex.Message,
                    ControlAppearance.Danger,
                    new SymbolIcon(SymbolRegular.ErrorCircle12),
                    TimeSpan.FromSeconds(5)
                );
            }
        }

        public async Task Shutdown()
        {
            WMI.Services.ICredential? credential = null;

            if (App.CREDENTIAL != null)
                credential = new WMI.Services.Credential() { UserName = App.CREDENTIAL.UserName, Password = App.CREDENTIAL.Password };

            try
            {
                await Task.Run(() =>
                {
                    using (var wmi = new WMIService(Computer.DnsHostName, credential, App.SERVER == null ? true : false))
                    {
                        wmi.Invoke("SELECT * FROM Win32_OperatingSystem", "Shutdown");
                    }
                });

                snackbarService.Show("Command",
                    "Shutdown command sent successfully",
                    ControlAppearance.Success,
                    new SymbolIcon(SymbolRegular.CheckmarkCircle12),
                    TimeSpan.FromSeconds(5)
                );
            }
            catch (Exception ex)
            {
                snackbarService.Show("Error",
                    ex.Message,
                    ControlAppearance.Danger,
                    new SymbolIcon(SymbolRegular.ErrorCircle12),
                    TimeSpan.FromSeconds(5)
                );
            }
        }

        public async Task Get()
        {
            ComputerEntry entry = Computer;

            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    using (var computersRepository = new ComputersRepository(ldap))
                    {
                        entry = await computersRepository.GetByCNAsync(Computer.CN);
                    }
                }
            });

            Computer = entry;
        }

    }

}