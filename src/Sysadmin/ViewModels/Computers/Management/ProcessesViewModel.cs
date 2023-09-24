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
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace Sysadmin.ViewModels
{
    public partial class ProcessesViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        private INavigationService _navigationService;
        private IExchangeService _exchangeService;

        [ObservableProperty]
        private ComputerEntry _computer = new ComputerEntry();

        [ObservableProperty]
        private IEnumerable<ProcessEntity> _items = new List<ProcessEntity>();

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        [ObservableProperty]
        private ProcessEntity _selectedItem;

        [ObservableProperty]
        private bool _isBusy = false;

        public ProcessesViewModel(INavigationService navigationService, IExchangeService exchangeService)
        {
            _navigationService = navigationService;
            _exchangeService = exchangeService;
        }

        public async void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();

            if (_exchangeService.GetParameter() is ComputerEntry entry)
            {
                Computer = entry;
                await Get(Computer.DnsHostName);
            }
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
            _navigationService.Navigate(typeof(Views.Pages.ComputerPage));
        }

        [RelayCommand]
        private async void OnRefresh()
        {
            await Get(Computer.DnsHostName);
        }

        [RelayCommand]
        private async void OnStop()
        {
            if (SelectedItem != null)
                await Stop(Computer.DnsHostName, SelectedItem.Handle);
        }

        public async Task Get(string computerAddress)
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
                    using (var wmi = new WMIService(computerAddress, credential))
                    {
                        List<Dictionary<string, object>> queryResult = wmi.Query("Select * From Win32_Process");

                        foreach (Dictionary<string, object> properties in queryResult)
                        {
                            ProcessEntity entity = WmiResolver<ProcessEntity>.GetValues(properties);
                            entities.Add(entity);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

            Items = entities.OrderBy(c => c.Caption);

            IsBusy = false;
        }

        public async Task Stop(string computerAddress, string handle)
        {
            IsBusy = true;

            ICredential? credential = null;

            if (App.CREDENTIAL != null)
                credential = new Credential() { UserName = App.CREDENTIAL.UserName, Password = App.CREDENTIAL.Password };

            try
            {
                await Task.Run(() =>
                {
                    using (var wmi = new WMIService(computerAddress, credential))
                    {
                        wmi.Invoke("Select * From Win32_Process WHERE Handle = '" + handle + "'", "Terminate");
                    }
                });

                await Get(computerAddress);
            }
            catch (Exception ex)
            {
                 ErrorMessage = ex.Message;
            }

            IsBusy = false;
        }

    }
}