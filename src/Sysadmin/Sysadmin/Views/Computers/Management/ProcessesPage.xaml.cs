using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Sysadmin.WMI.Models;
using SysAdmin.ActiveDirectory.Models;
using SysAdmin.ViewModels;
using System;
using System.Collections.Generic;
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
    public sealed partial class ProcessesPage : Page
    {

        public ComputerEntry Computer { get; set; }

        public ProcessesViewModel ViewModel { get; } = new ProcessesViewModel();

        public ProcessesPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is ComputerEntry)
            {
                Computer = (ComputerEntry)e.Parameter;
                await ViewModel.Get(Computer.DnsHostName);
            }
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            stopButton.IsEnabled = false;

            await ViewModel.Get(Computer.DnsHostName);
        }

        private async void stopButton_Click(object sender, RoutedEventArgs e)
        {
            stopButton.IsEnabled = false;

            await ViewModel.Stop(Computer.DnsHostName, (dataGrid.SelectedItem as ProcessEntity).Handle);
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
                stopButton.IsEnabled = true;
            else
                stopButton.IsEnabled = false;
        }
    }

}