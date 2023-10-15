using SysAdmin.Services;
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

        private ISettingsService settings;

        public ViewModels.ComputerViewModel ViewModel
        {
            get;
        }

        public ComputerPage(ViewModels.ComputerViewModel viewModel, ISettingsService settings)
        {
            ViewModel = viewModel;
            this.settings = settings;

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

        private void vnc_Click(object sender, RoutedEventArgs e)
        {
            string path = settings.VNCPath;
            string args = ViewModel.Computer.DnsHostName + ":" + settings.VNCPort.ToString();
            System.Diagnostics.Process.Start(path, args);
        }

        private void rdp_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void MemberOfControl_Changed()
        {
            await ViewModel.Get();
        }

        private void MemberOfControl_Error(string ErrorMessage)
        {
            snackbar.Message = ErrorMessage;
            snackbar.Show();
        }

    }
}