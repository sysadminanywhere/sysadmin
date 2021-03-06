using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Sysadmin.WMI;
using Sysadmin.WMI.Models;
using Sysadmin.WMI.Services;
using SysAdmin.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SysAdmin.ViewModels
{
    public class ProcessesViewModel : ObservableObject
    {

        public ObservableCollection<ProcessEntity> Items = new ObservableCollection<ProcessEntity>();

        INavigationService navigation = App.Current.Services.GetService<INavigationService>();
        INotificationService notification = App.Current.Services.GetService<INotificationService>();
        IBusyService busyService = App.Current.Services.GetService<IBusyService>();

        public async Task Get(string computerAddress)
        {
            busyService.Busy();

            Items.Clear();

            ICredential credential = null;

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
                            ProcessEntity entity = WMIResolver<ProcessEntity>.GetValues(properties);
                            entities.Add(entity);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                notification.ShowErrorMessage(ex.Message);
            }

            Items = new ObservableCollection<ProcessEntity>(entities.OrderBy(c => c.Caption));

            OnPropertyChanged(nameof(Items));

            busyService.Idle();
        }

        public async Task Stop(string computerAddress, string handle)
        {
            busyService.Busy();

            ICredential credential = null;

            if (App.CREDENTIAL != null)
                credential = new Credential() { UserName = App.CREDENTIAL.UserName, Password = App.CREDENTIAL.Password };

            List<ProcessEntity> entities = new List<ProcessEntity>();

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
                notification.ShowErrorMessage(ex.Message);
            }

            busyService.Idle();
        }

    }
}