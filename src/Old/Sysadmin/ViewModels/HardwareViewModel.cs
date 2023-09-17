using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Sysadmin.WMI;
using Sysadmin.WMI.Models.Hardware;
using Sysadmin.WMI.Services;
using SysAdmin.ActiveDirectory.Models;
using SysAdmin.Models;
using SysAdmin.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SysAdmin.ViewModels
{
    public class HardwareViewModel : ObservableObject
    {

        public ObservableCollection<HardwareItem> Items = new ObservableCollection<HardwareItem>();

        INavigationService navigation = App.Current.Services.GetService<INavigationService>();
        INotificationService notification = App.Current.Services.GetService<INotificationService>();
        IBusyService busyService = App.Current.Services.GetService<IBusyService>();

        private async Task<List<Dictionary<string, object>>> Query(string computerAddress, string queryString)
        {
            busyService.Busy();

            Items.Clear();

            ICredential credential = null;

            List<Dictionary<string, object>> queryResult = new List<Dictionary<string, object>>();

            if (App.CREDENTIAL != null)
                credential = new Credential() { UserName = App.CREDENTIAL.UserName, Password = App.CREDENTIAL.Password };

            List<HardwareItem> entities = new List<HardwareItem>();

            try
            {
                await Task.Run(() =>
                {
                    using (var wmi = new WMIService(computerAddress, credential))
                    {
                        queryResult = wmi.Query(queryString);
                    }
                });
            }
            catch (Exception ex)
            {
                notification.ShowErrorMessage(ex.Message);
            }

            busyService.Idle();

            return queryResult;
        }

        public async Task<List<DiskDriveEntity>> DiskDrive(ComputerEntry computer)
        {
            return await HardwareResolver<DiskDriveEntity>.GetValues(await Query(computer.DnsHostName, "SELECT * FROM Win32_DiskDrive"));
        }

        public async Task<OperatingSystemEntity> OperatingSystem(ComputerEntry computer)
        {
            return await HardwareResolver<OperatingSystemEntity>.GetValue(await Query(computer.DnsHostName, "SELECT * FROM Win32_OperatingSystem"));
        }

        public async Task<List<DiskPartitionEntity>> DiskPartition(ComputerEntry computer)
        {
            return await HardwareResolver<DiskPartitionEntity>.GetValues(await Query(computer.DnsHostName, "SELECT * FROM Win32_DiskPartition"));
        }

        public async Task<List<ProcessorEntity>> Processor(ComputerEntry computer)
        {
            return await HardwareResolver<ProcessorEntity>.GetValues(await Query(computer.DnsHostName, "SELECT * FROM Win32_Processor"));
        }

        public async Task<VideoControllerEntity> VideoController(ComputerEntry computer)
        {
            return await HardwareResolver<VideoControllerEntity>.GetValue(await Query(computer.DnsHostName, "SELECT * FROM Win32_VideoController"));
        }

        public async Task<List<PhysicalMemoryEntity>> PhysicalMemory(ComputerEntry computer)
        {
            return await HardwareResolver<PhysicalMemoryEntity>.GetValues(await Query(computer.DnsHostName, "SELECT * FROM Win32_PhysicalMemory"));
        }

        public async Task<List<LogicalDiskEntity>> LogicalDisk(ComputerEntry computer)
        {
            return await HardwareResolver<LogicalDiskEntity>.GetValues(await Query(computer.DnsHostName, "SELECT * FROM Win32_LogicalDisk"));
        }

        public async Task<BaseboardEntity> BaseBoard(ComputerEntry computer)
        {
            return await HardwareResolver<BaseboardEntity>.GetValue(await Query(computer.DnsHostName, "SELECT * FROM Win32_BaseBoard"));
        }

        public async Task<BIOSEntity> BIOS(ComputerEntry computer)
        {
            return await HardwareResolver<BIOSEntity>.GetValue(await Query(computer.DnsHostName, "SELECT * FROM Win32_BIOS"));
        }

        public async Task<ComputerSystemEntity> ComputerSystem(ComputerEntry computer)
        {
            return await HardwareResolver<ComputerSystemEntity>.GetValue(await Query(computer.DnsHostName, "SELECT * FROM Win32_ComputerSystem"));
        }
    }
}