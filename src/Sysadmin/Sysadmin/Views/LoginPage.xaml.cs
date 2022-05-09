using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SysAdmin.ActiveDirectory.Services.Ldap;
using SysAdmin.ViewModels;
using System;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SysAdmin.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {

        public event EventHandler Connecting;
        public event EventHandler Connected;
        public event EventHandler Error;

        public LoginViewModel ViewModel { get; } = new LoginViewModel();

        public LoginPage()
        {
            this.InitializeComponent();

            if (ApplicationData.Current.LocalSettings.Values["ServerName"] != null)
                serverName.Text = ApplicationData.Current.LocalSettings.Values["ServerName"].ToString();

            if (ApplicationData.Current.LocalSettings.Values["UserNameOther"] != null)
                userNameOther.Text = ApplicationData.Current.LocalSettings.Values["UserNameOther"].ToString();

            if (ApplicationData.Current.LocalSettings.Values["UserNameCredentials"] != null)
                userNameCredentials.Text = ApplicationData.Current.LocalSettings.Values["UserNameCredentials"].ToString();

            if (ApplicationData.Current.LocalSettings.Values["ServerPort"] != null)
                serverPort.Value = int.Parse(ApplicationData.Current.LocalSettings.Values["ServerPort"].ToString());

            if (ApplicationData.Current.LocalSettings.Values["IsSSL"] != null)
                sslCheck.IsChecked = bool.Parse(ApplicationData.Current.LocalSettings.Values["IsSSL"].ToString());
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {

            Connecting?.Invoke(this, EventArgs.Empty);

            switch (pivotLogin.SelectedIndex)
            {
                case 0:
                    App.SERVER = null;
                    if (toggleSwitch.IsOn)
                    {
                        App.CREDENTIAL = new Credential()
                        {
                            UserName = userNameCredentials.Text,
                            Password = passwordCredentials.Password
                        };
                    }
                    else
                    {
                        App.CREDENTIAL = null;
                    }
                    break;

                case 1:
                    App.SERVER = new SecureServer()
                    {
                        ServerName = serverName.Text,
                        Port = int.Parse(serverPort.Text),
                        IsSSL = (bool)sslCheck.IsChecked
                    };
                    App.CREDENTIAL = new Credential()
                    {
                        UserName = userNameOther.Text,
                        Password = passwordOther.Password,
                    };
                    break;
            }

            bool isConnected = await ViewModel.Login();

            if (isConnected)
            {
                ApplicationData.Current.LocalSettings.Values["ServerName"] = serverName.Text;
                ApplicationData.Current.LocalSettings.Values["UserNameOther"] = userNameOther.Text;
                ApplicationData.Current.LocalSettings.Values["UserNameCredentials"] = userNameCredentials.Text;
                ApplicationData.Current.LocalSettings.Values["ServerPort"] = serverPort.Value;
                ApplicationData.Current.LocalSettings.Values["IsSSL"] = sslCheck.IsChecked;

                Connected?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                Error?.Invoke(this, EventArgs.Empty);
            }
        }

        private void sslCheck_Checked(object sender, RoutedEventArgs e)
        {
            serverPort.Value = 636;
        }

        private void sslCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            serverPort.Value = 389;
        }

    }
}