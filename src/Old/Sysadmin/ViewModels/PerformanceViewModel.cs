using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Sysadmin.WMI;
using Sysadmin.WMI.Services;
using SysAdmin.Models;
using SysAdmin.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysAdmin.ViewModels
{
    public class PerformanceViewModel: ObservableObject
    {

        public ObservableCollection<NameValueItem> Items = new ObservableCollection<NameValueItem>();

        INavigationService navigation = App.Current.Services.GetService<INavigationService>();
        INotificationService notification = App.Current.Services.GetService<INotificationService>();
        IBusyService busyService = App.Current.Services.GetService<IBusyService>();

        UInt64 totalPhysicalMemory = 0;

        public async Task Init(string computerAddress)
        {
            busyService.Busy();

            Items.Clear();

            ICredential credential = null;

            if (App.CREDENTIAL != null)
                credential = new Credential() { UserName = App.CREDENTIAL.UserName, Password = App.CREDENTIAL.Password };

            try
            {
                await Task.Run(() =>
                {
                    using (var wmi = new WMIService(computerAddress, credential))
                    {
                        List<Dictionary<string, object>> queryResult = wmi.Query("Select * FROM Win32_ComputerSystem");
                        var totalMemory = queryResult[0].Where(c => c.Key == "TotalPhysicalMemory").FirstOrDefault();
                        if (totalMemory.Value != null)
                            totalPhysicalMemory = UInt64.Parse(totalMemory.Value.ToString());
                    }
                });
            }
            catch (Exception ex)
            {
                notification.ShowErrorMessage(ex.Message);
            }

            busyService.Idle();
        }

        public async Task Get(string computerAddress)
        {

            ICredential credential = null;

            if (App.CREDENTIAL != null)
                credential = new Credential() { UserName = App.CREDENTIAL.UserName, Password = App.CREDENTIAL.Password };

            List<NameValueItem> entities = new List<NameValueItem>();

            try
            {
                await Task.Run(() =>
                {
                    using (var wmi = new WMIService(computerAddress, credential))
                    {
                        // CPU

                        List<Dictionary<string, object>> queryProcessorResult = wmi.Query("Select * FROM Win32_PerfFormattedData_PerfOS_Processor");

                        foreach (Dictionary<string, object> item in queryProcessorResult)
                        {
                            if (item["Name"].ToString() == "_Total")
                            {
                                var result = item.Where(c => c.Key == "PercentProcessorTime").FirstOrDefault();
                                if (result.Value != null)
                                {
                                    entities.Add(new NameValueItem() { Name = "CPU", Value = result.Value.ToString() + "%" });
                                }
                            }
                        }                      


                        // Memory

                        List<Dictionary<string, object>> queryMemoryResult = wmi.Query("Select * FROM Win32_PerfFormattedData_PerfOS_Memory");

                        var availableBytesResult = queryMemoryResult[0].Where(c => c.Key == "AvailableBytes").FirstOrDefault();

                        if (availableBytesResult.Value != null)
                        {
                            UInt64 availableBytes = UInt64.Parse(availableBytesResult.Value.ToString());

                            UInt64 r = ((totalPhysicalMemory - availableBytes) * 100) / totalPhysicalMemory;

                            entities.Add(new NameValueItem() { Name = "Memory", Value = r.ToString() + "%" });
                        }


                        // Disk

                        List<Dictionary<string, object>> queryLogicalDiskResult = wmi.Query("Select * From Win32_LogicalDisk WHERE Caption='C:'");

                        var sizeItem = queryLogicalDiskResult[0].Where(c => c.Key == "Size").FirstOrDefault();
                        var freeSpaceItem = queryLogicalDiskResult[0].Where(c => c.Key == "FreeSpace").FirstOrDefault();

                        if (sizeItem.Value != null && freeSpaceItem.Value != null)
                        {
                            UInt64 size = UInt64.Parse(sizeItem.Value.ToString());
                            UInt64 freeSpace = UInt64.Parse(freeSpaceItem.Value.ToString());

                            UInt64 r = ((size - freeSpace) * 100) / size;

                            entities.Add(new NameValueItem() { Name = "C:", Value = r.ToString() + "%" });
                        }

                    }
                });
            }
            catch (Exception ex)
            {
                //notification.ShowErrorMessage(ex.Message);
            }

            Items = new ObservableCollection<NameValueItem>(entities);

            OnPropertyChanged(nameof(Items));

        }

    }

}
