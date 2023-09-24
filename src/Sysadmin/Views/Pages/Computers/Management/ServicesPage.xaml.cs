using Sysadmin.WMI.Models;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Controls;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class ServicesPage : INavigableView<ViewModels.ServicesViewModel>
    {
        public ViewModels.ServicesViewModel ViewModel
        {
            get;
        }

        public ServicesPage(ViewModels.ServicesViewModel viewModel)
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

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItem == null)
                return;

            string state = (dataGrid.SelectedItem as ServiceEntity).State;

            if (e.AddedItems.Count > 0)
            {
                if (state == "Running")
                {
                    startButton.IsEnabled = false;
                    stopButton.IsEnabled = true;
                }
                else
                {
                    startButton.IsEnabled = true;
                    stopButton.IsEnabled = false;
                }
            }
            else
            {
                startButton.IsEnabled = false;
                stopButton.IsEnabled = false;
            }
        }
    }
}
