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
using System.Text;
using System.Threading.Tasks;

namespace SysAdmin.ViewModels
{
    public class EventsViewModel : ObservableObject
    {

        public ObservableCollection<EventEntity> Items = new ObservableCollection<EventEntity>();

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

            List<EventEntity> entities = new List<EventEntity>();

            try
            {
                await Task.Run(() =>
                {
                    using (var wmi = new WMIService(computerAddress, credential))
                    {
                        string today = String.Format("{0:yyyyMMddHHmmss}.000000000", DateTime.Today);

                        List<Dictionary<string, object>> queryResult = wmi.Query("Select RecordNumber, EventType, EventCode, Type, TimeGenerated, SourceName, Category, Logfile, Message From Win32_NTLogEvent Where TimeGenerated > '" + today + "' And EventType = 1");

                        foreach (Dictionary<string, object> properties in queryResult)
                        {
                            EventEntity entity = WMIResolver<EventEntity>.GetValues(properties);
                            entities.Add(entity);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                notification.ShowErrorMessage(ex.Message);
            }

            Items = new ObservableCollection<EventEntity>(entities);

            OnPropertyChanged(nameof(Items));

            busyService.Idle();
        }

    }
}