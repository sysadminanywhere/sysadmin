using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SysAdmin.Views
{
    public sealed partial class SelectDirectoryDialog : ContentDialog
    {

        public string DistinguishedName { get; set; }

        public SelectDirectoryDialog()
        {
            this.InitializeComponent();

            IsPrimaryButtonEnabled = false;
        }

        private void DirectoryTreeControl_SelectedItem(string DistinguishedName)
        {
            if (!string.IsNullOrEmpty(DistinguishedName))
            {
                this.DistinguishedName = DistinguishedName;
                IsPrimaryButtonEnabled = true;
            }
            else
            {
                IsPrimaryButtonEnabled = false;
            }
        }
    }

}