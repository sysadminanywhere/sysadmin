using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Sysadmin.Services;
using Sysadmin.WMI.Services;
using SysAdmin.ActiveDirectory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wpf.Ui.Mvvm.Contracts;

namespace Sysadmin.ViewModels
{
    public partial class PerformanceViewModel : ViewModel
    {
        private bool _isInitialized = false;

        private INavigationService _navigationService;
        private IExchangeService _exchangeService;

        [ObservableProperty]
        private ComputerEntry _computer = new ComputerEntry();

        [ObservableProperty]
        private int _cpu = 0;

        [ObservableProperty]
        private int _memory = 0;

        [ObservableProperty]
        private int _disk = 0;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        [ObservableProperty]
        private bool _isBusy = false;

        [ObservableProperty]
        private bool _isClosed = false;

        public UInt64 totalPhysicalMemory = 0;

        public PerformanceViewModel(INavigationService navigationService, IExchangeService exchangeService)
        {
            _navigationService = navigationService;
            _exchangeService = exchangeService;
        }

        public override async void OnNavigatedTo()
        {
            IsClosed = false;

            if (!_isInitialized)
                InitializeViewModel();

            if (_exchangeService.GetParameter() is ComputerEntry entry)
            {
                Computer = entry;
                await Init(Computer.DnsHostName);
            }
        }

        public override void OnNavigatedFrom()
        {
            IsClosed = true;
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

        public async Task Init(string computerAddress)
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
                        List<Dictionary<string, object>> queryResult = wmi.Query("Select * FROM Win32_ComputerSystem");
                        if (queryResult.Count > 0)
                        {
                            var totalMemory = queryResult[0].FirstOrDefault(c => c.Key == "TotalPhysicalMemory");
                            if (totalMemory.Value != null)
                                totalPhysicalMemory = Convert.ToUInt64(totalMemory.Value);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

            IsBusy = false;
        }

        public async Task Update()
        {

            string computerAddress = Computer.DnsHostName;

            ICredential? credential = null;

            if (App.CREDENTIAL != null)
                credential = new Credential() { UserName = App.CREDENTIAL.UserName, Password = App.CREDENTIAL.Password };

            try
            {
                await Task.Run(() =>
                {
                    using (var wmi = new WMIService(computerAddress, credential, App.SERVER == null ? true : false))
                    {
                        // CPU

                        List<Dictionary<string, object>> queryProcessorResult = wmi.Query("Select * FROM Win32_PerfFormattedData_PerfOS_Processor");
                        if (queryProcessorResult.Count > 0)
                        {
                            foreach (Dictionary<string, object> item in queryProcessorResult)
                            {
                                if (item["Name"].ToString() == "_Total")
                                {
                                    var result = item.FirstOrDefault(c => c.Key == "PercentProcessorTime");
                                    if (result.Value != null)
                                    {
                                        Cpu = Convert.ToInt32(result.Value);
                                    }
                                }
                            }
                        }


                        // Memory

                        List<Dictionary<string, object>> queryMemoryResult = wmi.Query("Select * FROM Win32_PerfFormattedData_PerfOS_Memory");
                        if (queryMemoryResult.Count > 0)
                        {
                            var availableBytesResult = queryMemoryResult[0].FirstOrDefault(c => c.Key == "AvailableBytes");

                            if (availableBytesResult.Value != null)
                            {
                                UInt64 availableBytes = Convert.ToUInt64(availableBytesResult.Value);

                                UInt64 percent = ((totalPhysicalMemory - availableBytes) * 100) / totalPhysicalMemory;

                                Memory = Convert.ToInt32(percent);
                            }
                        }


                        // Disk

                        List<Dictionary<string, object>> queryLogicalDiskResult = wmi.Query("Select * From Win32_LogicalDisk WHERE Caption='C:'");
                        if (queryLogicalDiskResult.Count > 0)
                        {
                            var sizeItem = queryLogicalDiskResult[0].FirstOrDefault(c => c.Key == "Size");
                            var freeSpaceItem = queryLogicalDiskResult[0].FirstOrDefault(c => c.Key == "FreeSpace");

                            if (sizeItem.Value != null && freeSpaceItem.Value != null)
                            {
                                UInt64 size = Convert.ToUInt64(sizeItem.Value);
                                UInt64 freeSpace = Convert.ToUInt64(freeSpaceItem.Value);

                                UInt64 percent = ((size - freeSpace) * 100) / size;

                                Disk = Convert.ToInt32(percent);
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

        }
    }
}