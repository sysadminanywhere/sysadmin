using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Sysadmin.Services;
using Sysadmin.WMI;
using Sysadmin.WMI.Models.Hardware;
using Sysadmin.WMI.Services;
using SysAdmin.ActiveDirectory.Models;
using SysAdmin.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wpf.Ui;

namespace Sysadmin.ViewModels
{
    public partial class HardwareViewModel : ViewModel
    {
        private bool _isInitialized = false;

        private INavigationService _navigationService;
        private IExchangeService _exchangeService;

        [ObservableProperty]
        private ComputerEntry _computer = new ComputerEntry();

        [ObservableProperty]
        private IEnumerable<HardwareItem> _items = new List<HardwareItem>();

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        [ObservableProperty]
        private bool _isBusy = false;

        public HardwareViewModel(INavigationService navigationService, IExchangeService exchangeService)
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

        private async Task<List<Dictionary<string, object>>> Query(string computerAddress, string queryString)
        {
            IsBusy = true;

            ICredential? credential = null;

            List<Dictionary<string, object>> queryResult = new List<Dictionary<string, object>>();

            if (App.CREDENTIAL != null)
                credential = new Credential() { UserName = App.CREDENTIAL.UserName, Password = App.CREDENTIAL.Password };

            try
            {
                await Task.Run(() =>
                {
                    using (var wmi = new WMIService(computerAddress, credential, App.SERVER == null ? true : false))
                    {
                        queryResult = wmi.Query(queryString);
                    }
                });
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

            IsBusy = false;

            return queryResult;
        }

        public async Task<List<DiskDriveEntity>> DiskDrive()
        {
            return await HardwareResolver<DiskDriveEntity>.GetValues(await Query(Computer.DnsHostName, "SELECT * FROM Win32_DiskDrive"));
        }

        public async Task<OperatingSystemEntity> OperatingSystem()
        {
            return await HardwareResolver<OperatingSystemEntity>.GetValue(await Query(Computer.DnsHostName, "SELECT * FROM Win32_OperatingSystem"));
        }

        public async Task<List<DiskPartitionEntity>> DiskPartition()
        {
            return await HardwareResolver<DiskPartitionEntity>.GetValues(await Query(Computer.DnsHostName, "SELECT * FROM Win32_DiskPartition"));
        }

        public async Task<List<ProcessorEntity>> Processor()
        {
            return await HardwareResolver<ProcessorEntity>.GetValues(await Query(Computer.DnsHostName, "SELECT * FROM Win32_Processor"));
        }

        public async Task<VideoControllerEntity> VideoController()
        {
            return await HardwareResolver<VideoControllerEntity>.GetValue(await Query(Computer.DnsHostName, "SELECT * FROM Win32_VideoController"));
        }

        public async Task<List<PhysicalMemoryEntity>> PhysicalMemory()
        {
            return await HardwareResolver<PhysicalMemoryEntity>.GetValues(await Query(Computer.DnsHostName, "SELECT * FROM Win32_PhysicalMemory"));
        }

        public async Task<List<LogicalDiskEntity>> LogicalDisk()
        {
            return await HardwareResolver<LogicalDiskEntity>.GetValues(await Query(Computer.DnsHostName, "SELECT * FROM Win32_LogicalDisk"));
        }

        public async Task<BaseboardEntity> BaseBoard()
        {
            return await HardwareResolver<BaseboardEntity>.GetValue(await Query(Computer.DnsHostName, "SELECT * FROM Win32_BaseBoard"));
        }

        public async Task<BIOSEntity> BIOS()
        {
            return await HardwareResolver<BIOSEntity>.GetValue(await Query(Computer.DnsHostName, "SELECT * FROM Win32_BIOS"));
        }

        public async Task<ComputerSystemEntity> ComputerSystem()
        {
            return await HardwareResolver<ComputerSystemEntity>.GetValue(await Query(Computer.DnsHostName, "SELECT * FROM Win32_ComputerSystem"));
        }
    }

}