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
    public sealed partial class ResetPasswordDialog : ContentDialog, IResetPasswordDialog
    {

        public UserEntry User { get; set; } = new UserEntry();

        public string Password { get; set; }

        public ResetPasswordDialog()
        {
            this.InitializeComponent();

            if (ApplicationData.Current.LocalSettings.Values["UserDefaultPassword"] != null)
            {
                Password = ApplicationData.Current.LocalSettings.Values["UserDefaultPassword"].ToString();
            }
        }

        public async Task<bool?> ShowDialog(object xamlRoot)
        {
            this.XamlRoot = (XamlRoot)xamlRoot;
            var result = await this.ShowAsync();

            if (result == ContentDialogResult.Primary)
                return true;
            else
                return false;
        }

    }
}
