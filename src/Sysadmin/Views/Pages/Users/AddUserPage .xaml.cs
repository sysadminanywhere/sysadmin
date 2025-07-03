using SysAdmin.Services;
using System;
using System.Text.RegularExpressions;
using Wpf.Ui.Controls;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class AddUserPage : INavigableView<ViewModels.AddUserViewModel>
    {

        private ISettingsService _settingsService;

        public ViewModels.AddUserViewModel ViewModel
        {
            get;
        }

        public AddUserPage(ViewModels.AddUserViewModel viewModel, ISettingsService settingsService)
        {
            ViewModel = viewModel;
            _settingsService = settingsService;

            InitializeComponent();

            directoryControl.DistinguishedName = App.CONTAINERS.GetUsersContainer();

            if (!string.IsNullOrEmpty(_settingsService.UserDefaultPassword))
            {
                txtPassword.Password = _settingsService.UserDefaultPassword;
                txtConfirmPassword.Password = _settingsService.UserDefaultPassword;
            }
        }

        private void txtDisplayName_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            string _DisplayNameValue = txtDisplayName.Text;
            string _DisplayNameValue2 = string.Empty;

            if (_settingsService.UserDisplayNameFormat.Contains("<FirstName>"))
            {
                txtFirstName.Text = Regex.Replace(_DisplayNameValue, _settingsService.UserDisplayNameFormat, "${FirstName}", RegexOptions.None, TimeSpan.FromMilliseconds(100));
                _DisplayNameValue2 += txtFirstName.Text;
            }

            if (_settingsService.UserDisplayNameFormat.Contains("<Middle>"))
            {
                txtInitials.Text = Regex.Replace(_DisplayNameValue, _settingsService.UserDisplayNameFormat, "${Middle}", RegexOptions.None, TimeSpan.FromMilliseconds(100));
            }

            if (_settingsService.UserDisplayNameFormat.Contains("<LastName>"))
            {
                txtLastName.Text = Regex.Replace(_DisplayNameValue, _settingsService.UserDisplayNameFormat, "${LastName}", RegexOptions.None, TimeSpan.FromMilliseconds(100));
                _DisplayNameValue2 += " " + txtLastName.Text;
            }

            txtAccountName.Text = Regex.Replace(_DisplayNameValue2, _settingsService.UserLoginPattern, _settingsService.UserLoginFormat, RegexOptions.None, TimeSpan.FromMilliseconds(100)).ToLower();
            ViewModel.User.SamAccountName = txtAccountName.Text;

            ViewModel.User.FirstName = txtFirstName.Text;
            ViewModel.User.Initials = txtInitials.Text;
            ViewModel.User.LastName = txtLastName.Text;
        }

        private void chkUserMustChangePassword_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (chkPasswordNeverExpires != null)
            {
                chkPasswordNeverExpires.IsEnabled = false;
                chkPasswordNeverExpires.IsChecked = false;
            }
        }

        private void chkUserMustChangePassword_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            chkPasswordNeverExpires.IsEnabled = true;
        }

        private void chkPasswordNeverExpires_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            chkUserMustChangePassword.IsEnabled = false;
            chkUserMustChangePassword.IsChecked = false;
        }

        private void chkPasswordNeverExpires_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            chkUserMustChangePassword.IsEnabled = true;
        }

        private void txtPassword_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ViewModel != null)
            {
                ViewModel.Password = ((System.Windows.Controls.PasswordBox)sender).SecurePassword;
            }
            if (!string.IsNullOrEmpty(txtPassword.Password) && !string.IsNullOrEmpty(txtConfirmPassword.Password) && txtPassword.Password == txtConfirmPassword.Password)
            {
                btnOK.IsEnabled = true;
            }
            else
            {
                btnOK.IsEnabled = false;
            }
        }

        private void txtConfirmPassword_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPassword.Password) && !string.IsNullOrEmpty(txtConfirmPassword.Password) && txtPassword.Password == txtConfirmPassword.Password)
            {
                btnOK.IsEnabled = true;
            }
            else
            {
                btnOK.IsEnabled = false;
            }
        }
    }

}