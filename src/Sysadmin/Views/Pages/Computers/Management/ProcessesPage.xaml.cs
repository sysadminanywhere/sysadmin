using System.Windows.Controls;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class ProcessesPage : Wpf.Ui.Controls.INavigableView<ViewModels.ProcessesViewModel>
    {
        public ViewModels.ProcessesViewModel ViewModel
        {
            get;
        }

        public ProcessesPage(ViewModels.ProcessesViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
                stopButton.IsEnabled = true;
            else
                stopButton.IsEnabled = false;
        }

    }
}
