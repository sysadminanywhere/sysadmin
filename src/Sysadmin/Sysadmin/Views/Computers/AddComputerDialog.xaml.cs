using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SysAdmin.ActiveDirectory.Models;
using SysAdmin.Services.Dialogs;
using System;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SysAdmin.Views.Computers
{
    public sealed partial class AddComputerDialog : ContentDialog, IAddComputerDialogService
    {

        public ComputerEntry Computer { get; set; } = new ComputerEntry();
        public bool IsAccountEnabled { get; set; } = true;

        public string DistinguishedName { get; set; }

        public AddComputerDialog()
        {
            this.InitializeComponent();
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
    }
}
