using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;
using Sysadmin.WMI.Models.Hardware;
using SysAdmin.ActiveDirectory.Models;
using SysAdmin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SysAdmin.Views.Computers.Management
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HardwarePage : Page
    {

        public HardwareViewModel ViewModel { get; } = new HardwareViewModel();

        public ComputerEntry Computer { get; set; }

        public HardwarePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is ComputerEntry)
            {
                Computer = (ComputerEntry)e.Parameter;
            }
        }

        private async void ListBoxItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ListBoxItem item = (ListBoxItem)sender;

            switch (item.Tag)
            {
                case "computersystem":
                    GetHardwares(await ViewModel.ComputerSystem(Computer));
                    break;
                case "bios":
                    GetHardwares(await ViewModel.BIOS(Computer));
                    break;
                case "baseboard":
                    GetHardwares(await ViewModel.BaseBoard(Computer));
                    break;
                case "diskdrive":
                    GetHardwares(await ViewModel.DiskDrive(Computer));
                    break;
                case "operatingsystem":
                    GetHardwares(await ViewModel.OperatingSystem(Computer));
                    break;
                case "diskpartition":
                    GetHardwares(await ViewModel.DiskPartition(Computer));
                    break;
                case "processor":
                    GetHardwares(await ViewModel.Processor(Computer));
                    break;
                case "videocontroller":
                    GetHardwares(await ViewModel.VideoController(Computer));
                    break;
                case "physicalmemory":
                    GetHardwares(await ViewModel.PhysicalMemory(Computer));
                    break;
                case "logicaldisk":
                    GetHardwares(await ViewModel.LogicalDisk(Computer));
                    break;
            }
        }

        private void GetHardwares(object hardware)
        {
            if (hardware.GetType().Name.StartsWith("List"))
            {
                comboBox.Items.Clear();

                List<IHardware> list = new List<IHardware>((IEnumerable<IHardware>)hardware);

                foreach (IHardware item in list)
                {
                    comboBox.Items.Add(item);
                }

                comboBox.Visibility = Visibility.Visible;
            }
            else
            {
                comboBox.Visibility = Visibility.Collapsed;

                ShowProperties(hardware);
            }
        }

        private void ShowProperties(object hardware)
        {
            if (hardware != null)
            {
                foreach (PropertyInfo propertyInfo in hardware.GetType().GetProperties())
                {
                    string text = String.Empty;
                    var value = propertyInfo.GetValue(hardware, null);

                    if (value != null)
                    {
                        if (value is List<string>)
                        {
                            List<string> items = (List<string>)value;
                            if (items.Count > 0)
                                text = items.Select(a => a.ToString()).Aggregate((i, j) => i + ", " + j);
                            else
                                text = string.Empty;
                        }
                        else
                        {
                            text = value.ToString();
                        }
                    }

                    string name = Regex.Replace(propertyInfo.Name, @"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))", " $0");

                    ViewModel.Items.Add(new Models.HardwareItem() { Name = name, Value = text });
                }
            }
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowProperties(comboBox.SelectedItem);
        }

    }
}
