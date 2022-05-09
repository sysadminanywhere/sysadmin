using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SysAdmin.ActiveDirectory.Models;
using SysAdmin.Services.Dialogs;
using System;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SysAdmin.Views.Contacts
{
    public sealed partial class AddContactDialog : ContentDialog, IAddContactDialogService
    {

        public ContactEntry Contact { get; set; } = new ContactEntry();

        public AddContactDialog()
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
    }
}