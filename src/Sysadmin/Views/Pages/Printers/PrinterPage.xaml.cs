using System.Windows;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class PrinterPage : Wpf.Ui.Controls.INavigableView<ViewModels.PrinterViewModel>
    {
        public ViewModels.PrinterViewModel ViewModel
        {
            get;
        }

        public PrinterPage(ViewModels.PrinterViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to delete this printer?", "Delete", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
                ViewModel.DeleteCommand.Execute(ViewModel);
        }

    }
}
