using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SysAdmin.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {

        public string Version
        {
            get
            {
                var version = Windows.ApplicationModel.Package.Current.Id.Version;
                return string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
            }
        }

        public SettingsPage()
        {
            this.InitializeComponent();

            Patterns();
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["ThemeSetting"] = ((ToggleSwitch)sender).IsOn ? 0 : 1;
        }

        private void ToggleSwitch_Loaded(object sender, RoutedEventArgs e)
        {
            ((ToggleSwitch)sender).IsOn = App.Current.RequestedTheme == ApplicationTheme.Light;
        }

        private void cmbDisplayNamePattern_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                switch (cmbDisplayNamePattern.SelectedIndex)
                {
                    case 0: // First Last (e.g. Homer Simpson)
                        ApplicationData.Current.LocalSettings.Values["UserDisplayNameFormat"] = @"(?<FirstName>\S+) (?<LastName>\S+)";
                        break;

                    case 1: // Last First (e.g. Simpson Homer)
                        ApplicationData.Current.LocalSettings.Values["UserDisplayNameFormat"] = @"(?<LastName>\S+) (?<FirstName>\S+)";
                        break;

                    case 2: // First Middle Last (e.g. Homer Jay Simpson)
                        ApplicationData.Current.LocalSettings.Values["UserDisplayNameFormat"] = @"(?<FirstName>\S+) (?<Middle>\S+) (?<LastName>\S+)";
                        break;

                    case 3: // Last First Middle (e.g. Simpson Homer Jay)
                        ApplicationData.Current.LocalSettings.Values["UserDisplayNameFormat"] = @"(?<LastName>\S+) (?<FirstName>\S+) (?<Middle>\S+)";
                        break;

                    default: // None
                        ApplicationData.Current.LocalSettings.Values["UserDisplayNameFormat"] = string.Empty;
                        break;
                }

            }
            catch { }
        }

        private void cmbLoginPattern_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                switch (cmbLoginPattern.SelectedIndex)
                {
                    case 0: // FLast (e.g. hsimpson)
                        ApplicationData.Current.LocalSettings.Values["UserLoginPattern"] = @"(?<FirstName>\S)\S+ (?<LastName>\S+)";
                        ApplicationData.Current.LocalSettings.Values["UserLoginFormat"] = @"${FirstName}${LastName}";
                        break;

                    case 1: // F.Last (e.g. h.simpson)
                        ApplicationData.Current.LocalSettings.Values["UserLoginPattern"] = @"(?<FirstName>\S)\S+ (?<LastName>\S+)";
                        ApplicationData.Current.LocalSettings.Values["UserLoginFormat"] = @"${FirstName}.${LastName}";
                        break;

                    case 2: // First.Last (e.g. homer.simpson)
                        ApplicationData.Current.LocalSettings.Values["UserLoginPattern"] = @"(?<FirstName>\S+) (?<LastName>\S+)";
                        ApplicationData.Current.LocalSettings.Values["UserLoginFormat"] = @"${FirstName}.${LastName}";
                        break;

                    case 3: // Last (e.g. simpson)
                        ApplicationData.Current.LocalSettings.Values["UserLoginPattern"] = @"(?<FirstName>\S+) (?<LastName>\S+)";
                        ApplicationData.Current.LocalSettings.Values["UserLoginFormat"] = @"${ LastName}";
                        break;

                    case 4: // LastF (e.g. simpsonh)
                        ApplicationData.Current.LocalSettings.Values["UserLoginPattern"] = @"(?<FirstName>\S)\S+ (?<LastName>\S+)";
                        ApplicationData.Current.LocalSettings.Values["UserLoginFormat"] = @"${LastName}${FirstName}";
                        break;

                    default: // None
                        ApplicationData.Current.LocalSettings.Values["UserLoginPattern"] = string.Empty;
                        ApplicationData.Current.LocalSettings.Values["UserLoginFormat"] = string.Empty;
                        break;
                }

            }
            catch { }
        }

        private void txtDefaultPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["UserDefaultPassword"] = txtDefaultPassword.Text;
        }

        private void Patterns()
        {
            try
            {
                if (ApplicationData.Current.LocalSettings.Values["UserDisplayNameFormat"] != null)
                {
                    switch (ApplicationData.Current.LocalSettings.Values["UserDisplayNameFormat"].ToString())
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

                if (ApplicationData.Current.LocalSettings.Values["UserLoginPattern"] != null && ApplicationData.Current.LocalSettings.Values["UserLoginFormat"] != null)
                {
                    if (ApplicationData.Current.LocalSettings.Values["UserLoginPattern"].ToString() == @"(?<FirstName>\S)\S+ (?<LastName>\S+)" & ApplicationData.Current.LocalSettings.Values["UserLoginFormat"].ToString() == @"${FirstName}${LastName}")
                    {
                        cmbLoginPattern.SelectedIndex = 0;
                    }
                    else if (ApplicationData.Current.LocalSettings.Values["UserLoginPattern"].ToString() == @"(?<FirstName>\S)\S+ (?<LastName>\S+)" & ApplicationData.Current.LocalSettings.Values["UserLoginFormat"].ToString() == @"${FirstName}.${LastName}")
                    {
                        cmbLoginPattern.SelectedIndex = 1;
                    }
                    else if (ApplicationData.Current.LocalSettings.Values["UserLoginPattern"].ToString() == @"(?<FirstName>\S+) (?<LastName>\S+)" & ApplicationData.Current.LocalSettings.Values["UserLoginFormat"].ToString() == @"${FirstName}.${LastName}")
                    {
                        cmbLoginPattern.SelectedIndex = 2;
                    }
                    else if (ApplicationData.Current.LocalSettings.Values["UserLoginPattern"].ToString() == @"(?<FirstName>\S+) (?<LastName>\S+)" & ApplicationData.Current.LocalSettings.Values["UserLoginFormat"].ToString() == @"${LastName}")
                    {
                        cmbLoginPattern.SelectedIndex = 3;
                    }
                    else if (ApplicationData.Current.LocalSettings.Values["UserLoginPattern"].ToString() == @"(?<FirstName>\S)\S+ (?<LastName>\S+)" & ApplicationData.Current.LocalSettings.Values["UserLoginFormat"].ToString() == @"${LastName}${FirstName}")
                    {
                        cmbLoginPattern.SelectedIndex = 4;
                    }
                    else
                    {
                        cmbLoginPattern.SelectedIndex = cmbLoginPattern.Items.Count - 1;
                    }
                }

                if (ApplicationData.Current.LocalSettings.Values["UserDefaultPassword"] != null)
                    txtDefaultPassword.Text = ApplicationData.Current.LocalSettings.Values["UserDefaultPassword"].ToString();
            }
            catch { }
        }

    }
}