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
using Wpf.Ui.Mvvm.Contracts;

namespace Sysadmin.ViewModels
{
    public partial class SoftwareViewModel : ViewModel
    {
        private bool _isInitialized = false;

        private INavigationService _navigationService;
        private IExchangeService _exchangeService;

        [ObservableProperty]
        private ComputerEntry _computer = new ComputerEntry();

        [ObservableProperty]
        private IEnumerable<SoftwareEntity> _items = new List<SoftwareEntity>();

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        [ObservableProperty]
        private bool _isBusy = false;

        public SoftwareViewModel(INavigationService navigationService, IExchangeService exchangeService)
        {
            _navigationService = navigationService;
            _exchangeService = exchangeService;
        }

        public override async void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();

            if (_exchangeService.GetParameter() is ComputerEntry entry)
            {
                Computer = entry;
                await Get(Computer.DnsHostName);
            }
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

        public async Task Get(string computerAddress)
        {
            IsBusy = true;

            ICredential? credential = null;

            if (App.CREDENTIAL != null)
                credential = new Credential() { UserName = App.CREDENTIAL.UserName, Password = App.CREDENTIAL.Password };

            List<SoftwareEntity> entities = new List<SoftwareEntity>();

            try
            {
                await Task.Run(() =>
                {
                    using (var wmi = new WMIService(computerAddress, credential, App.SERVER == null ? true : false))
                    {
                        List<Dictionary<string, object>> queryResult = wmi.Query("Select * From Win32_Product");

                        foreach (Dictionary<string, object> properties in queryResult)
                        {
                            SoftwareEntity entity = WmiResolver<SoftwareEntity>.GetValues(properties);
                            entities.Add(entity);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

            Items = entities.OrderBy(c => c.Name);

            IsBusy = false;
        }

    }
}