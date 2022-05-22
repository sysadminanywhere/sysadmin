using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SysAdmin.Services.Dialogs;
using System;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SysAdmin.Views
{
    public sealed partial class RenameDialog : ContentDialog, IRenameDialogService
    {

        public string CN { get; set; }

        public string DistinguishedName { get; set; }

        public RenameDialog()
        {
            this.InitializeComponent();
        }

        public async Task<bool?> ShowDialog(string distinguishedName, string cn)
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
