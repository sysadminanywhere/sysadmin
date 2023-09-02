using SysAdmin.Services;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Controls;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class AddUserPage : INavigableView<ViewModels.UserViewModel>
    {

        private ISettingsService _settingsService;

        public ViewModels.UserViewModel ViewModel
        {
            get;
        }

        public AddUserPage(ViewModels.UserViewModel viewModel, ISettingsService settingsService)
        {
            ViewModel = viewModel;
            _settingsService = settingsService;

            InitializeComponent();

            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            directoryControl.DistinguishedName = App.CONTAINERS.GetUsersContainer();

            if (!string.IsNullOrEmpty(_settingsService.UserDefaultPassword))
            {
                txtPassword.Password = _settingsService.UserDefaultPassword;
                txtConfirmPassword.Password = _settingsService.UserDefaultPassword;
            }
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ErrorMessage")
            {
                snackbar.Message = ViewModel.ErrorMessage;
                snackbar.Show();
            }
        }

        private void txtDisplayName_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            string _DisplayNameValue = txtDisplayName.Text;
            string _DisplayNameValue2 = string.Empty;

            if (_settingsService.UserDisplayNameFormat.Contains("<FirstName>"))
            {
                txtFirstName.Text = Regex.Replace(_DisplayNameValue, _settingsService.UserDisplayNameFormat, "${FirstName}");
                _DisplayNameValue2 += txtFirstName.Text;
            }

            if (_settingsService.UserDisplayNameFormat.Contains("<Middle>"))
            {
                txtInitials.Text = Regex.Replace(_DisplayNameValue, _settingsService.UserDisplayNameFormat, "${Middle}");
            }

            if (_settingsService.UserDisplayNameFormat.Contains("<LastName>"))
            {
                txtLastName.Text = Regex.Replace(_DisplayNameValue, _settingsService.UserDisplayNameFormat, "${LastName}");
                _DisplayNameValue2 += " " + txtLastName.Text;
            }

            txtAccountName.Text = Regex.Replace(_DisplayNameValue2, _settingsService.UserLoginPattern, _settingsService.UserLoginFormat).ToLower();
            ViewModel.User.SamAccountName = txtAccountName.Text;

        }

        private void chkUserMustChangePassword_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (chkPasswordNeverExpires != null)
            {
                if (chkUserMustChangePassword.IsChecked == true)
                {
                    chkPasswordNeverExpires.IsEnabled = false;
                    chkPasswordNeverExpires.IsChecked = false;
                }
                else
                {
                    chkPasswordNeverExpires.IsEnabled = true;
                }
            }
        }

        private void chkPasswordNeverExpires_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (chkUserMustChangePassword != null)
            {
                if (chkPasswordNeverExpires.IsChecked == true)
                {
                    chkUserMustChangePassword.IsEnabled = false;
                    chkUserMustChangePassword.IsChecked = false;
                }
                else
                {
                    chkUserMustChangePassword.IsEnabled = true;
                }
            }
        }

        private void txtPassword_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ViewModel != null)
            {
                ViewModel.Password = ((System.Windows.Controls.PasswordBox)sender).SecurePassword;
            }
        }

    }
}
