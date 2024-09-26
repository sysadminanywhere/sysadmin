using SysAdmin.Services;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class ComputerPage : INavigableView<ViewModels.ComputerViewModel>
    {

        private ISettingsService settings;
        private INavigationService navigationService;

        public ViewModels.ComputerViewModel ViewModel
        {
            get;
        }

        public ComputerPage(ViewModels.ComputerViewModel viewModel, ISettingsService settings, INavigationService navigationService)
        {
            ViewModel = viewModel;

            this.settings = settings;
            this.navigationService = navigationService;

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

            if (e.PropertyName == "Computer")
            {
                memberOf.MemberOf = ViewModel.Computer.MemberOf;
                memberOf.PrimaryGroupId = ViewModel.Computer.PrimaryGroupId;
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

            if (File.Exists(path))
            {
                System.Diagnostics.Process.Start(path, args);   //NOSONAR
            }
            else
            {
                var result = MessageBox.Show("The VNС viewer is not installed or is located in a different location. Fix the path?", "Remote desktop", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    navigationService.Navigate(typeof(SettingsPage));
                }
            }
        }

        private void rdp_Click(object sender, RoutedEventArgs e)
        {
            string args = "/v:" + ViewModel.Computer.DnsHostName;
            System.Diagnostics.Process.Start("mstsc", args);   //NOSONAR
        }

        private async void MemberOfControl_Changed()    //NOSONAR
        {
            await ViewModel.Get();
        }

        private void MemberOfControl_Error(string ErrorMessage)     //NOSONAR
        {
            snackbar.Message = ErrorMessage;
            snackbar.Show();
        }

    }
}