using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SysAdmin.ActiveDirectory.Models;
using SysAdmin.Services;
using SysAdmin.ViewModels;
using SysAdmin.Views.Computers.Management;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SysAdmin.Views.Computers
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ComputerDetailsPage : Page
    {
        public ComputersViewModel ViewModel { get; } = new ComputersViewModel();

        INotificationService notification = App.Current.Services.GetService<INotificationService>();

        public ComputerDetailsPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is ComputerEntry)
            {
                var computer = (ComputerEntry)e.Parameter;
                await ViewModel.Get(computer.CN);
            }
        }

        private async void MemberOfControl_Changed()
        {
            await ViewModel.Get(ViewModel.Computer.CN);
        }

        private void mnuProcesses_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ViewModel.Computer.DnsHostName))
                notification.ShowErrorMessage("DNS address of the computer is incorrect!");
            else
                Frame.Navigate(typeof(ProcessesPage), ViewModel.Computer);
        }

        private void mnuServices_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ViewModel.Computer.DnsHostName))
                notification.ShowErrorMessage("DNS address of the computer is incorrect!");
            else
                Frame.Navigate(typeof(ServicesPage), ViewModel.Computer);
        }

        private void mnuEvents_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ViewModel.Computer.DnsHostName))
                notification.ShowErrorMessage("DNS address of the computer is incorrect!");
            else
                Frame.Navigate(typeof(EventsPage), ViewModel.Computer);
        }

        private void mnuSoftware_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ViewModel.Computer.DnsHostName))
                notification.ShowErrorMessage("DNS address of the computer is incorrect!");
            else
                Frame.Navigate(typeof(SoftwarePage), ViewModel.Computer);
        }

        private void mnuHardware_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ViewModel.Computer.DnsHostName))
                notification.ShowErrorMessage("DNS address of the computer is incorrect!");
            else
                Frame.Navigate(typeof(HardwarePage), ViewModel.Computer);
        }

    }
}