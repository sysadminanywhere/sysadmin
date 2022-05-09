using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SysAdmin.ActiveDirectory.Models;
using SysAdmin.Services.Dialogs;
using System;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SysAdmin.Views.Groups
{
    public sealed partial class AddGroupDialog : ContentDialog, IAddGroupDialogService
    {

        public GroupEntry Group { get; set; } = new GroupEntry();
        public GroupScopes GroupScope { get; set; } = GroupScopes.Global;
        public bool IsSecurity { get; set; } = true;

        public AddGroupDialog()
        {
            this.InitializeComponent();
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

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            IsSecurity = (bool)radSecurity.IsChecked;

            if (cmbScope.SelectedIndex == 0)
                GroupScope = GroupScopes.Global;

            if (cmbScope.SelectedIndex == 1)
                GroupScope = GroupScopes.Local;

            if (cmbScope.SelectedIndex == 2)
                GroupScope = GroupScopes.Universal;
        }
    }
}
