using Sysadmin.WMI.Models.Hardware;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common.Interfaces;
using SysAdmin.Models;
using System.Linq;
using System.Collections;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class HardwarePage : INavigableView<ViewModels.HardwareViewModel>
    {
        public ViewModels.HardwareViewModel ViewModel
        {
            get;
        }

        public HardwarePage(ViewModels.HardwareViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();

            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ErrorMessage")
            {
                snackbar.Message = ViewModel.ErrorMessage;
                snackbar.Show();
            }
        }

        private async void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            try
            {
                ListBoxItem item = (ListBoxItem)sender;

                switch (item.Tag)
                {
                    case "computersystem":
                        GetHardwares(await ViewModel.ComputerSystem());
                        break;
                    case "bios":
                        GetHardwares(await ViewModel.BIOS());
                        break;
                    case "baseboard":
                        GetHardwares(await ViewModel.BaseBoard());
                        break;
                    case "diskdrive":
                        GetHardwares(await ViewModel.DiskDrive());
                        break;
                    case "operatingsystem":
                        GetHardwares(await ViewModel.OperatingSystem());
                        break;
                    case "diskpartition":
                        GetHardwares(await ViewModel.DiskPartition());
                        break;
                    case "processor":
                        GetHardwares(await ViewModel.Processor());
                        break;
                    case "videocontroller":
                        GetHardwares(await ViewModel.VideoController());
                        break;
                    case "physicalmemory":
                        GetHardwares(await ViewModel.PhysicalMemory());
                        break;
                    case "logicaldisk":
                        GetHardwares(await ViewModel.LogicalDisk());
                        break;
                }
            }
            catch (Exception ex)
            {
                snackbar.Message = ex.Message;
                snackbar.Show();
            }
        }

        private void GetHardwares(object hardware)
        {
            ViewModel.Items = new List<HardwareItem>();

            try
            {
                if (hardware is IEnumerable<IHardware> list)
                {
                    comboBox.Items.Clear();
                    comboBox.Text = "-- Select an item --";

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
            catch (Exception ex)
            {
                snackbar.Message = ex.Message;
                snackbar.Show();
            }
        }

        private void ShowProperties(object hardware)
        {
            if (hardware != null)
            {
                try
                {

                    List<HardwareItem> list = new List<HardwareItem>();

                    foreach (PropertyInfo propertyInfo in hardware.GetType().GetProperties())
                    {
                        string text = string.Empty;
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

                        list.Add(new HardwareItem() { Name = name, Value = text });
                    }

                    ViewModel.Items = list;
                }
                catch (Exception ex)
                {
                    snackbar.Message = ex.Message;
                    snackbar.Show();
                }
            }
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowProperties(comboBox.SelectedItem);
        }

    }
}