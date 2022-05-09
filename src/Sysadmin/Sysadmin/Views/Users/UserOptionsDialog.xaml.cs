using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SysAdmin.ActiveDirectory;
using SysAdmin.ActiveDirectory.Models;
using SysAdmin.Services.Dialogs;
using System;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SysAdmin.Views.Users
{
    public sealed partial class UserOptionsDialog : ContentDialog, IUserOptionsDialogService
    {

        public UserEntry User { get; set; } = new UserEntry();

        public bool IsCannotChangePassword { get; set; }
        public bool IsPasswordNeverExpires { get; set; }
        public bool IsAccountDisabled { get; set; }
        public bool IsMustChangePassword { get; set; }

        public UserOptionsDialog()
        {
            this.InitializeComponent();
        }

        private void Init()
        {
            if (User != null)
            {
                UserAccountControls userAccountControl = User.UserControl;

                IsCannotChangePassword = (userAccountControl & UserAccountControls.PASSWD_CANT_CHANGE) == UserAccountControls.PASSWD_CANT_CHANGE;
                IsPasswordNeverExpires = (userAccountControl & UserAccountControls.DONT_EXPIRE_PASSWD) == UserAccountControls.DONT_EXPIRE_PASSWD;
                IsAccountDisabled = (userAccountControl & UserAccountControls.ACCOUNTDISABLE) == UserAccountControls.ACCOUNTDISABLE;

                /*
                    pwdLastSet : If the value is set to 0 ...
                    userAccountControl : and the UF_DONT_EXPIRE_PASSWD flag is not set.
                */

                bool isDontExpirePassword = (userAccountControl & UserAccountControls.DONT_EXPIRE_PASSWD) == UserAccountControls.DONT_EXPIRE_PASSWD;
                if (isDontExpirePassword == false && User.PasswordLastSet == new DateTime(1601, 01, 01, 0, 0, 0, DateTimeKind.Utc))
                    IsMustChangePassword = true;
                else
                    IsMustChangePassword = false;

                chkUserCannotChangePassword.IsOn = IsCannotChangePassword;
                chkPasswordNeverExpires.IsOn = IsPasswordNeverExpires;
                chkAccountDisabled.IsOn = IsAccountDisabled;
                chkUserMustChangePassword.IsOn = IsMustChangePassword;
            }
        }

        public async Task<bool?> ShowDialog(object xamlRoot)
        {
            Init();

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
    }
}
