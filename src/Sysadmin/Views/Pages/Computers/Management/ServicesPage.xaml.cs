using Sysadmin.WMI.Models;
using System.Windows.Controls;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class ServicesPage : Wpf.Ui.Controls.INavigableView<ViewModels.ServicesViewModel>
    {
        public ViewModels.ServicesViewModel ViewModel
        {
            get;
        }

        public ServicesPage(ViewModels.ServicesViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
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
