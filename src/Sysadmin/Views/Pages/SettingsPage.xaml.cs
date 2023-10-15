using Microsoft.Extensions.DependencyInjection;
using SysAdmin.Services;
using System;
using System.Windows.Controls;
using Wpf.Ui.Common.Interfaces;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : INavigableView<ViewModels.SettingsViewModel>
    {

        ISettingsService settings;

        public ViewModels.SettingsViewModel ViewModel
        {
            get;
        }

        public SettingsPage(ViewModels.SettingsViewModel viewModel, IServiceProvider serviceProvider)
        {
            ViewModel = viewModel;

            settings = serviceProvider.GetService<ISettingsService>();

            InitializeComponent();

            Patterns();
        }

        private void cmbDisplayNamePattern_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cmbDisplayNamePattern.SelectedIndex)
            {
                case 0: // First Last (e.g. Homer Simpson)
                    settings.UserDisplayNameFormat = @"(?<FirstName>\S+) (?<LastName>\S+)";
                    break;

                case 1: // Last First (e.g. Simpson Homer)
                    settings.UserDisplayNameFormat = @"(?<LastName>\S+) (?<FirstName>\S+)";
                    break;

                case 2: // First Middle Last (e.g. Homer Jay Simpson)
                    settings.UserDisplayNameFormat = @"(?<FirstName>\S+) (?<Middle>\S+) (?<LastName>\S+)";
                    break;

                case 3: // Last First Middle (e.g. Simpson Homer Jay)
                    settings.UserDisplayNameFormat = @"(?<LastName>\S+) (?<FirstName>\S+) (?<Middle>\S+)";
                    break;

                default: // None
                    settings.UserDisplayNameFormat = string.Empty;
                    break;
            }

        }

        private void cmbLoginPattern_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cmbLoginPattern.SelectedIndex)
            {
                case 0: // FLast (e.g. hsimpson)
                    settings.UserLoginPattern = @"(?<FirstName>\S)\S+ (?<LastName>\S+)";
                    settings.UserLoginFormat = @"${FirstName}${LastName}";
                    break;

                case 1: // F.Last (e.g. h.simpson)
                    settings.UserLoginPattern = @"(?<FirstName>\S)\S+ (?<LastName>\S+)";
                    settings.UserLoginFormat = @"${FirstName}.${LastName}";
                    break;

                case 2: // First.Last (e.g. homer.simpson)
                    settings.UserLoginPattern = @"(?<FirstName>\S+) (?<LastName>\S+)";
                    settings.UserLoginFormat = @"${FirstName}.${LastName}";
                    break;

                case 3: // Last (e.g. simpson)
                    settings.UserLoginPattern = @"(?<FirstName>\S+) (?<LastName>\S+)";
                    settings.UserLoginFormat = @"${ LastName}";
                    break;

                case 4: // LastF (e.g. simpsonh)
                    settings.UserLoginPattern = @"(?<FirstName>\S)\S+ (?<LastName>\S+)";
                    settings.UserLoginFormat = @"${LastName}${FirstName}";
                    break;

                default: // None
                    settings.UserLoginPattern = string.Empty;
                    settings.UserLoginFormat = string.Empty;
                    break;
            }

        }

        private void txtDefaultPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            settings.UserDefaultPassword = txtDefaultPassword.Text;
        }

        private void Patterns()
        {
            if (!string.IsNullOrEmpty(settings.UserDisplayNameFormat))
            {
                switch (settings.UserDisplayNameFormat.ToString())
                {
                    case @"(?<FirstName>\S+) (?<LastName>\S+)":
                        cmbDisplayNamePattern.SelectedIndex = 0;
                        break;

                    case @"(?<LastName>\S+) (?<FirstName>\S+)":
                        cmbDisplayNamePattern.SelectedIndex = 1;
                        break;

                    case @"(?<FirstName>\S+) (?<Middle>\S+) (?<LastName>\S+)":
                        cmbDisplayNamePattern.SelectedIndex = 2;
                        break;

                    case @"(?<LastName>\S+) (?<FirstName>\S+) (?<Middle>\S+)":
                        cmbDisplayNamePattern.SelectedIndex = 3;
                        break;

                    default:
                        cmbDisplayNamePattern.SelectedIndex = cmbDisplayNamePattern.Items.Count - 1;
                        break;
                }
            }

            if (!string.IsNullOrEmpty(settings.UserLoginPattern) && !string.IsNullOrEmpty(settings.UserLoginFormat))
            {
                if (settings.UserLoginPattern == @"(?<FirstName>\S)\S+ (?<LastName>\S+)" && settings.UserLoginFormat == @"${FirstName}${LastName}")
                {
                    cmbLoginPattern.SelectedIndex = 0;
                }
                else if (settings.UserLoginPattern == @"(?<FirstName>\S)\S+ (?<LastName>\S+)" && settings.UserLoginFormat == @"${FirstName}.${LastName}")
                {
                    cmbLoginPattern.SelectedIndex = 1;
                }
                else if (settings.UserLoginPattern == @"(?<FirstName>\S+) (?<LastName>\S+)" && settings.UserLoginFormat == @"${FirstName}.${LastName}")
                {
                    cmbLoginPattern.SelectedIndex = 2;
                }
                else if (settings.UserLoginPattern == @"(?<FirstName>\S+) (?<LastName>\S+)" && settings.UserLoginFormat == @"${LastName}")
                {
                    cmbLoginPattern.SelectedIndex = 3;
                }
                else if (settings.UserLoginPattern == @"(?<FirstName>\S)\S+ (?<LastName>\S+)" && settings.UserLoginFormat == @"${LastName}${FirstName}")
                {
                    cmbLoginPattern.SelectedIndex = 4;
                }
                else
                {
                    cmbLoginPattern.SelectedIndex = cmbLoginPattern.Items.Count - 1;
                }
            }

            txtDefaultPassword.Text = settings.UserDefaultPassword;

            txtVNVPath.Text = settings.VNCPath;
            numVNVPort.Value = settings.VNCPort;
        }

        private void txtVNVPath_TextChanged(object sender, TextChangedEventArgs e)
        {
            settings.VNCPath = txtVNVPath.Text;
        }

        private void numVNVPort_ValueChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            settings.VNCPort = (int)numVNVPort.Value;
        }

    }
}