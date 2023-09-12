using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common.Interfaces;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class PrinterPage : INavigableView<ViewModels.PrinterViewModel>
    {
        public ViewModels.PrinterViewModel ViewModel
        {
            get;
        }

        public PrinterPage(ViewModels.PrinterViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure?", "Delete", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
                ViewModel.DeleteCommand.Execute(ViewModel);
        }

    }
}
