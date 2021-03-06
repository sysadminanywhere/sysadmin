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
    public class SoftwareViewModel : ObservableObject
    {

        public ObservableCollection<SoftwareEntity> Items = new ObservableCollection<SoftwareEntity>();

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

            List<SoftwareEntity> entities = new List<SoftwareEntity>();

            try
            {
                await Task.Run(() =>
                {
                    using (var wmi = new WMIService(computerAddress, credential))
                    {
                        List<Dictionary<string, object>> queryResult = wmi.Query("Select * From Win32_Product");

                        foreach (Dictionary<string, object> properties in queryResult)
                        {
                            SoftwareEntity entity = WMIResolver<SoftwareEntity>.GetValues(properties);
                            entities.Add(entity);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                notification.ShowErrorMessage(ex.Message);
            }

            Items = new ObservableCollection<SoftwareEntity>(entities.OrderBy(c => c.Name));

            OnPropertyChanged(nameof(Items));

            busyService.Idle();
        }

    }
}