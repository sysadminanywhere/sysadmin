using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Sysadmin.Services;
using Sysadmin.WMI;
using Sysadmin.WMI.Models;
using Sysadmin.WMI.Services;
using SysAdmin.ActiveDirectory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace Sysadmin.ViewModels
{
    public partial class ServicesViewModel : ViewModel
    {
        private bool isInitialized = false;

        private INavigationService navigationService;
        private IExchangeService exchangeService;
        private ISnackbarService snackbarService;

        [ObservableProperty]
        private ComputerEntry _computer = new ComputerEntry();

        [ObservableProperty]
        private IEnumerable<ServiceEntity> _items = new List<ServiceEntity>();

        [ObservableProperty]
        private ServiceEntity _selectedItem;

        [ObservableProperty]
        private bool _isBusy = false;

        public ServicesViewModel(INavigationService navigationService, IExchangeService exchangeService, ISnackbarService snackbarService)
        {
            this.navigationService = navigationService;
            this.exchangeService = exchangeService;
            this.snackbarService = snackbarService;
        }

        public override async void OnNavigatedTo()
        {
            if (!isInitialized)
                InitializeViewModel();

            if (exchangeService.GetParameter() is ComputerEntry entry)
            {
                Computer = entry;
                await Get(Computer.DnsHostName);
            }
        }

        private void InitializeViewModel()
        {
            isInitialized = true;
        }

        [RelayCommand]
        private async void OnRefresh()
        {
            await Get(Computer.DnsHostName);
        }

        [RelayCommand]
        private async void OnStart()
        {
            if (SelectedItem != null)
                await Start(Computer.DnsHostName, SelectedItem.ProcessId);
        }

        [RelayCommand]
        private async void OnStop()
        {
            if (SelectedItem != null)
                await Stop(Computer.DnsHostName, SelectedItem.ProcessId);
        }

        public async Task Get(string computerAddress)
        {
            IsBusy = true;

            ICredential? credential = null;

            if (App.CREDENTIAL != null)
                credential = new Credential() { UserName = App.CREDENTIAL.UserName, Password = App.CREDENTIAL.Password };

            List<ServiceEntity> entities = new List<ServiceEntity>();

            try
            {
                await Task.Run(() =>
                {
                    using (var wmi = new WMIService(computerAddress, credential, App.SERVER == null ? true : false))
                    {
                        List<Dictionary<string, object>> queryResult = wmi.Query("Select * From Win32_Service");

                        foreach (Dictionary<string, object> properties in queryResult)
                        {
                            ServiceEntity entity = WmiResolver<ServiceEntity>.GetValues(properties);
                            entities.Add(entity);
                        }
                    }
                });
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

            Items = entities.OrderBy(c => c.Caption);

            IsBusy = false;
        }

        public async Task Start(string computerAddress, string processId)
        {
            IsBusy = true;

            ICredential? credential = null;

            if (App.CREDENTIAL != null)
                credential = new Credential() { UserName = App.CREDENTIAL.UserName, Password = App.CREDENTIAL.Password };

            try
            {
                await Task.Run(() =>
                {
                    using (var wmi = new WMIService(computerAddress, credential, App.SERVER == null ? true : false))
                    {
                        wmi.Invoke("Select * From Win32_Service Where ProcessId = '" + processId + "'", "StartService");
                    }
                });

                await Get(computerAddress);
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

            IsBusy = false;
        }

        public async Task Stop(string computerAddress, string processId)
        {
            IsBusy = true;

            ICredential? credential = null;

            if (App.CREDENTIAL != null)
                credential = new Credential() { UserName = App.CREDENTIAL.UserName, Password = App.CREDENTIAL.Password };

            List<ProcessEntity> entities = new List<ProcessEntity>();

            try
            {
                await Task.Run(() =>
                {
                    using (var wmi = new WMIService(computerAddress, credential, App.SERVER == null ? true : false))
                    {
                        wmi.Invoke("Select * From Win32_Service Where ProcessId = '" + processId + "'", "StopService");
                    }
                });

                await Get(computerAddress);
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

            IsBusy = false;
        }

    }
}