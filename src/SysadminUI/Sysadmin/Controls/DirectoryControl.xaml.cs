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
    public partial class DirectoryControl : UserControl
    {

        public static readonly DependencyProperty DistinguishedNameProperty = DependencyProperty.Register(
            "DistinguishedName", typeof(string),
            typeof(DirectoryControl)
            );

        public string DistinguishedName
        {
            get => (string)GetValue(DistinguishedNameProperty);
            set
            {
                SetValue(DistinguishedNameProperty, value);
                distinguishedName.Text = value;
            }
        }

        public DirectoryControl()
        {
            this.InitializeComponent();
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            flyout.Show();
            directoryTreeControl.Load();
        }

        private void directoryTreeControl_SelectedItem(string DistinguishedName)
        {
            if (!string.IsNullOrEmpty(DistinguishedName))
            {
                distinguishedName.Text = DistinguishedName;
                this.DistinguishedName = DistinguishedName;
            }

            flyout.Hide();
        }

    }

}