using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Sysadmin.Services;
using Sysadmin.WMI;
using Sysadmin.WMI.Models;
using Sysadmin.WMI.Services;
using SysAdmin.ActiveDirectory.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace Sysadmin.ViewModels
{
    public partial class EventsViewModel : ViewModel
    {
        private bool isInitialized = false;

        private INavigationService navigationService;
        private IExchangeService exchangeService;
        private ISnackbarService snackbarService;

        [ObservableProperty]
        private ComputerEntry _computer = new ComputerEntry();

        [ObservableProperty]
        private IEnumerable<EventEntity> _items = new List<EventEntity>();

        [ObservableProperty]
        private bool _isBusy = false;

        public EventsViewModel(INavigationService navigationService, IExchangeService exchangeService, ISnackbarService snackbarService)
        {
            this.navigationService = navigationService;
            this.exchangeService = exchangeService;
            this.snackbarService = snackbarService;
        }

        public override async void OnNavigatedTo()
        {
            if (!isInitialized)
                InitializeViewModel();

            if (exchangeService.GetParameter() is ComputerEntry entry)
            {
                Computer = entry;
                await Get(Computer.DnsHostName, EventsFilter.TodayErrors);
            }
        }

        private void InitializeViewModel()
        {
            isInitialized = true;
        }

        [RelayCommand]
        private async void OnFilter(MenuItem menu)
        {
            EventsFilter filters = EventsFilter.TodayErrors;

            switch (menu.Tag)
            {
                case "todayerrors":
                    filters = EventsFilter.TodayErrors;
                    break;
                case "todaywarnings":
                    filters = EventsFilter.TodayWarnings;
                    break;
                case "todayinformations":
                    filters = EventsFilter.TodayInformations;
                    break;
                case "todaysecurityauditsuccess":
                    filters = EventsFilter.TodaySecurityAuditSuccess;
                    break;
                case "todaysecurityauditfailure":
                    filters = EventsFilter.TodaySecurityAuditFailure;
                    break;
                case "todayall":
                    filters = EventsFilter.TodayAll;
                    break;
            }

            await Get(Computer.DnsHostName, filters);
        }

        public async Task Get(string computerAddress, EventsFilter filter)
        {
            IsBusy = true;

            ICredential? credential = null;

            if (App.CREDENTIAL != null)
                credential = new Credential() { UserName = App.CREDENTIAL.UserName, Password = App.CREDENTIAL.Password };

            List<EventEntity> entities = new List<EventEntity>();

            try
            {
                await Task.Run(() =>
                {
                    using (var wmi = new WMIService(computerAddress, credential, App.SERVER == null ? true : false))
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
                            EventEntity entity = WmiResolver<EventEntity>.GetValues(properties);
                            entities.Add(entity);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                snackbarService.Show("Error",
                    ex.Message,
                    ControlAppearance.Danger,
                    new SymbolIcon(SymbolRegular.ErrorCircle12),
                    TimeSpan.FromSeconds(5)
                );
            }

            Items = entities;

            IsBusy = false;
        }

    }
}