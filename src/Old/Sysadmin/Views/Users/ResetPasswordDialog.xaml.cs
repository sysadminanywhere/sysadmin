using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SysAdmin.ActiveDirectory.Models;
using SysAdmin.Services;
using SysAdmin.Services.Dialogs;
using System;
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

        ISettingsService settings = App.Current.Services.GetService<ISettingsService>();

        public ResetPasswordDialog()
        {
            this.InitializeComponent();

            Password = settings.UserDefaultPassword;
        }

        public async Task<bool?> ShowDialog()
        {
            this.XamlRoot = App.GetMainWindow().Content.XamlRoot;

            var result = await this.ShowAsync();

            if (result == ContentDialogResult.Primary)
                return true;
            else
                return false;
        }

    }
}
