using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SysAdmin.ActiveDirectory.Models;
using SysAdmin.Services.Dialogs;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SysAdmin.Views.Users
{
    public sealed partial class AddUserDialog : ContentDialog, IAddUserDialogService
    {

        private string UserDisplayNameFormat = ApplicationData.Current.LocalSettings.Values["UserDisplayNameFormat"].ToString();
        private string UserLoginFormat = ApplicationData.Current.LocalSettings.Values["UserLoginFormat"].ToString();
        private string UserLoginPattern = ApplicationData.Current.LocalSettings.Values["UserLoginPattern"].ToString();

        public UserEntry User { get; set; } = new UserEntry();

        public string DistinguishedName { get; set; }

        public string Password { get; set; }
        public bool IsCannotChangePassword { get; set; }
        public bool IsPasswordNeverExpires { get; set; }
        public bool IsAccountDisabled { get; set; }
        public bool IsMustChangePassword { get; set; }

        public AddUserDialog()
        {
            this.InitializeComponent();

            if (ApplicationData.Current.LocalSettings.Values["UserDefaultPassword"] != null)
            {
                Password = ApplicationData.Current.LocalSettings.Values["UserDefaultPassword"].ToString();
                txtConfirmPassword.Password = Password;
            }
        }

        public async Task<bool?> ShowDialog(string distinguishedName, object xamlRoot)
        {
            this.DistinguishedName = distinguishedName;

            this.XamlRoot = (XamlRoot)xamlRoot;
            var result = await this.ShowAsync();

            if (result == ContentDialogResult.Primary)
                return true;
            else
                return false;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            IsCannotChangePassword = chkUserCannotChangePassword.IsOn;
            IsPasswordNeverExpires = chkPasswordNeverExpires.IsOn;
            IsAccountDisabled = chkAccountDisabled.IsOn;
            IsMustChangePassword = chkUserMustChangePassword.IsOn;

            User.DisplayName = txtDisplayName.Text;
            User.FirstName = txtFirstName.Text;
            User.Initials = txtInitials.Text;
            User.LastName = txtLastName.Text;
            User.CN = txtDisplayName.Text;
            User.Name = txtDisplayName.Text;
            User.SamAccountName = txtAccountName.Text;
        }

        private void chkUserMustChangePassword_Toggled(object sender, RoutedEventArgs e)
        {
            if (chkPasswordNeverExpires != null)
            {
                if (chkUserMustChangePassword.IsOn)
                {
                    chkPasswordNeverExpires.IsEnabled = false;
                    chkPasswordNeverExpires.IsOn = false;
                }
                else
                {
                    chkPasswordNeverExpires.IsEnabled = true;
                }
            }
        }

        private void chkPasswordNeverExpires_Toggled(object sender, RoutedEventArgs e)
        {
            if (chkUserMustChangePassword != null)
            {
                if (chkPasswordNeverExpires.IsOn)
                {
                    chkUserMustChangePassword.IsEnabled = false;
                    chkUserMustChangePassword.IsOn = false;
                }
                else
                {
                    chkUserMustChangePassword.IsEnabled = true;
                }
            }
        }

        private void txtDisplayName_KeyUp(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            try
            {
                string _DisplayNameValue = txtDisplayName.Text;
                string _DisplayNameValue2 = string.Empty;

                if (UserDisplayNameFormat.Contains("<FirstName>"))
                {
                    txtFirstName.Text = Regex.Replace(_DisplayNameValue, UserDisplayNameFormat, "${FirstName}");
                    _DisplayNameValue2 += txtFirstName.Text;
                }

                if (UserDisplayNameFormat.Contains("<Middle>"))
                {
                    txtInitials.Text = Regex.Replace(_DisplayNameValue, UserDisplayNameFormat, "${Middle}");
                }

                if (UserDisplayNameFormat.Contains("<LastName>"))
                {
                    txtLastName.Text = Regex.Replace(_DisplayNameValue, UserDisplayNameFormat, "${LastName}");
                    _DisplayNameValue2 += " " + txtLastName.Text;
                }

                txtAccountName.Text = Regex.Replace(_DisplayNameValue2, UserLoginPattern, UserLoginFormat).ToLower();

            }
            catch { }
        }
    }
}
