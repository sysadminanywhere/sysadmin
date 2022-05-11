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

        public async Task Get(string computerAddress, EventsFilter filter)
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
                        string today = string.Format("{0:yyyyMMddHHmmss}.000000000", DateTime.Today);

                        string queryString = "Select RecordNumber, EventType, EventCode, Type, TimeGenerated, SourceName, Category, Logfile, Message From Win32_NTLogEvent Where TimeGenerated > '" + today + "'"; ;

                        switch (filter)
                        {
                            case EventsFilter.TodayErrors:
                                queryString += " And EventType = 1";
                                break;

                            case EventsFilter.TodayWarnings:
                                queryString += " And EventType = 2";
                                break;

                            case EventsFilter.TodayInformations:
                                queryString += " And EventType = 3";
                                break;

                            case EventsFilter.TodaySecurityAuditSuccess:
                                queryString += " And EventType = 4";
                                break;

                            case EventsFilter.TodaySecurityAuditFailure:
                                queryString += " And EventType = 5";
                                break;
                        }

                        List<Dictionary<string, object>> queryResult = wmi.Query(queryString);

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