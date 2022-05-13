﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Sysadmin.WMI.Models;
using SysAdmin.ActiveDirectory.Models;
using SysAdmin.Models;
using SysAdmin.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SysAdmin.Views.Computers.Management
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EventsPage : Page
    {

        EventsFilter filter = EventsFilter.TodayErrors;

        public ComputerEntry Computer { get; set; }

        public EventsViewModel ViewModel { get; } = new EventsViewModel();

        public EventsPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is ComputerEntry)
            {
                Computer = (ComputerEntry)e.Parameter;
                await ViewModel.Get(Computer.DnsHostName, filter);
            }
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.Get(Computer.DnsHostName, filter);
        }

        private async void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem flyoutItem = (MenuFlyoutItem)e.OriginalSource;

            switch (flyoutItem.Tag)
            {
                case "todayerrors":
                    filter = EventsFilter.TodayErrors;
                    break;

                case "todaywarnings":
                    filter = EventsFilter.TodayWarnings;
                    break;

                case "todayinformations":
                    filter = EventsFilter.TodayInformations;
                    break;

                case "todaysecurityauditsuccess":
                    filter = EventsFilter.TodaySecurityAuditSuccess;
                    break;

                case "todaysecurityauditfailure":
                    filter = EventsFilter.TodaySecurityAuditFailure;
                    break;

                case "todayall":
                    filter = EventsFilter.TodayAll;
                    break;
            }
            await ViewModel.Get(Computer.DnsHostName, filter);
        }
    }
}