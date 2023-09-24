using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common.Interfaces;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class ComputerPage : INavigableView<ViewModels.ComputerViewModel>
    {
        public ViewModels.ComputerViewModel ViewModel
        {
            get;
        }

        public ComputerPage(ViewModels.ComputerViewModel viewModel)
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

            if (e.PropertyName == "SuccessMessage")
            {
                snackbar.Message = ViewModel.SuccessMessage;
                snackbarOk.Show();
            }
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to delete this computer?", "Delete", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
                ViewModel.DeleteCommand.Execute(ViewModel);
        }

        private void RebootMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to reboot this computer?", "Reboot", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
                ViewModel.RebootCommand.Execute(ViewModel);
        }

        private void ShutdownMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to shutdown this computer?", "Shutdown", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
                ViewModel.ShutdownCommand.Execute(ViewModel);
        }

    }
}
