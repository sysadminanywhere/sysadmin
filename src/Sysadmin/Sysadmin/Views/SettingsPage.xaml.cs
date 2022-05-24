using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SysAdmin.Services;
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

        ISettingsService settings = App.Current.Services.GetService<ISettingsService>();

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
            settings.ThemeSetting = ((ToggleSwitch)sender).IsOn ? 0 : 1;
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
            catch { }
        }

        private void cmbLoginPattern_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
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
            catch { }
        }

        private void txtDefaultPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            settings.UserDefaultPassword = txtDefaultPassword.Text;
        }

        private void Patterns()
        {
            try
            {
                if (settings.UserDisplayNameFormat != null)
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

                if (settings.UserLoginPattern != null && settings.UserLoginFormat != null)
                {
                    if (settings.UserLoginPattern == @"(?<FirstName>\S)\S+ (?<LastName>\S+)" & settings.UserLoginFormat == @"${FirstName}${LastName}")
                    {
                        cmbLoginPattern.SelectedIndex = 0;
                    }
                    else if (settings.UserLoginPattern == @"(?<FirstName>\S)\S+ (?<LastName>\S+)" & settings.UserLoginFormat == @"${FirstName}.${LastName}")
                    {
                        cmbLoginPattern.SelectedIndex = 1;
                    }
                    else if (settings.UserLoginPattern == @"(?<FirstName>\S+) (?<LastName>\S+)" & settings.UserLoginFormat == @"${FirstName}.${LastName}")
                    {
                        cmbLoginPattern.SelectedIndex = 2;
                    }
                    else if (settings.UserLoginPattern == @"(?<FirstName>\S+) (?<LastName>\S+)" & settings.UserLoginFormat == @"${LastName}")
                    {
                        cmbLoginPattern.SelectedIndex = 3;
                    }
                    else if (settings.UserLoginPattern == @"(?<FirstName>\S)\S+ (?<LastName>\S+)" & settings.UserLoginFormat == @"${LastName}${FirstName}")
                    {
                        cmbLoginPattern.SelectedIndex = 4;
                    }
                    else
                    {
                        cmbLoginPattern.SelectedIndex = cmbLoginPattern.Items.Count - 1;
                    }
                }

                txtDefaultPassword.Text = settings.UserDefaultPassword;
            }
            catch { }
        }

    }
}