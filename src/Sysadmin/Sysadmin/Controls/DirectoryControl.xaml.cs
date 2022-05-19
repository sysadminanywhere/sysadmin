using Microsoft.UI.Xaml.Controls;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SysAdmin.Controls
{
    public sealed partial class DirectoryControl : UserControl, INotifyPropertyChanged
    {

        private string distinguishedName { get; set; }

        public string DistinguishedName
        {
            get
            {
                return distinguishedName;
            }

            set
            {
                if (value != distinguishedName)
                {
                    distinguishedName = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public DirectoryControl()
        {
            this.InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void DirectoryTreeControl_SelectedItem(string DistinguishedName)
        {
            if (!string.IsNullOrEmpty(DistinguishedName))
            {
                this.DistinguishedName = DistinguishedName;
            }

            if (this.btnSelect.Flyout is Flyout f)
            {
                f.Hide();
            }
        }
    }
}
