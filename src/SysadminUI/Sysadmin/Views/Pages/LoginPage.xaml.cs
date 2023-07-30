using System;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Controls;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class LoginPage : INavigableView<ViewModels.LoginViewModel>
    {

        public ViewModels.LoginViewModel ViewModel
        {
            get;
        }

        public LoginPage(ViewModels.LoginViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();

            ViewModel.PropertyChanging += ViewModel_PropertyChanging;
        }

        private void ViewModel_PropertyChanging(object? sender, System.ComponentModel.PropertyChangingEventArgs e)
        {
            if (e.PropertyName == "ShowError")
            {
                snackbarError.Show();
            }
            if (e.PropertyName == "ShowConnecting")
            {
                snackbarConnecting.Show();
            }
        }

        private void password_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ViewModel != null)
            {
                ViewModel.Password = ((System.Windows.Controls.PasswordBox)sender).SecurePassword;
            }
        }
    }

}
