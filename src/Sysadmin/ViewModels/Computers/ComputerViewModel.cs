using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LdapForNet;
using Sysadmin.Services;
using Sysadmin.WMI.Services;
using SysAdmin.ActiveDirectory.Models;
using SysAdmin.ActiveDirectory.Repositories;
using SysAdmin.ActiveDirectory.Services.Ldap;
using System;
using System.Threading.Tasks;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace Sysadmin.ViewModels
{
    public partial class ComputerViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        private INavigationService _navigationService;
        private IExchangeService _exchangeService;

        [ObservableProperty]
        private ComputerEntry _computer = new ComputerEntry();

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        [ObservableProperty]
        private string _successMessage = string.Empty;

        public ComputerViewModel(INavigationService navigationService, IExchangeService exchangeService)
        {
            _navigationService = navigationService;
            _exchangeService = exchangeService;
        }

        public void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();

            if (_exchangeService.GetParameter() is ComputerEntry entry)
                Computer = entry;
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
        private void OnEdit()
        {
            _navigationService.Navigate(typeof(Views.Pages.EditComputerPage));
        }

        [RelayCommand]
        private void OnEvents()
        {
            _exchangeService.SetParameter(Computer);
            _navigationService.Navigate(typeof(Views.Pages.EventsPage));
        }

        [RelayCommand]
        private void OnServices()
        {
            _exchangeService.SetParameter(Computer);
            _navigationService.Navigate(typeof(Views.Pages.ServicesPage));
        }

        [RelayCommand]
        private void OnProcesses()
        {
            _exchangeService.SetParameter(Computer);
            _navigationService.Navigate(typeof(Views.Pages.ProcessesPage));
        }

        [RelayCommand]
        private void OnSoftware()
        {
            _exchangeService.SetParameter(Computer);
            _navigationService.Navigate(typeof(Views.Pages.SoftwarePage));
        }

        [RelayCommand]
        private void OnHardware()
        {
            _exchangeService.SetParameter(Computer);
            _navigationService.Navigate(typeof(Views.Pages.HardwarePage));
        }

        [RelayCommand]
        private void OnPerformance()
        {
            _exchangeService.SetParameter(Computer);
            _navigationService.Navigate(typeof(Views.Pages.PerformancePage));
        }

        [RelayCommand]
        private async Task OnDelete()
        {
            try
            {
                await Delete(Computer);
                _navigationService.Navigate(typeof(Views.Pages.ComputersPage));
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
                    using (var wmi = new WMIService(Computer.DnsHostName, credential))
                    {
                        wmi.Invoke("SELECT * FROM Win32_OperatingSystem", "Reboot");
                    }
                });

                SuccessMessage = "Reboot command sent successfully";
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
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
                    using (var wmi = new WMIService(Computer.DnsHostName, credential))
                    {
                        wmi.Invoke("SELECT * FROM Win32_OperatingSystem", "Shutdown");
                    }
                });

                SuccessMessage = "Shutdown command sent successfully";
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

    }

}