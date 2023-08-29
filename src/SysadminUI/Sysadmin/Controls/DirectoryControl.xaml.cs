using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace Sysadmin.Controls
{
    /// <summary>
    /// Interaction logic for DirectoryControl.xaml
    /// </summary>
    public partial class DirectoryControl : UserControl, INotifyPropertyChanged
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

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            flyout.Show();
        }
    }

}