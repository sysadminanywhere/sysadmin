using System;
using System.Windows;
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

        private void password_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ViewModel != null)
            {
                ViewModel.Password = ((System.Windows.Controls.PasswordBox)sender).SecurePassword;
            }
        }

        private void sslCheck_Checked(object sender, RoutedEventArgs e)
        {
            ViewModel.Port = "636";
        }

        private void sslCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            ViewModel.Port = "389";
        }

    }

}
