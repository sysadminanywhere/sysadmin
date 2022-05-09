using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SysAdmin.ActiveDirectory.Services.Ldap;
using SysAdmin.Services;
using SysAdmin.Views;
using System;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SysAdmin
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {

        public bool IsBackEnabled { get { return CanGoBack(); } }

        public MainWindow()
        {
            this.InitializeComponent();
            this.ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);

            // Init services with real controls

            NavigationService navigation = (NavigationService)App.Current.Services.GetService<INavigationService>();
            navigation.SetFrame(contentFrame);

            NotificationService notification = (NotificationService)App.Current.Services.GetService<INotificationService>();
            notification.SetInfoBar(infoBar);

            BusyService busyService = (BusyService)App.Current.Services.GetService<IBusyService>();
            busyService.SetProgressRing(progressRing);
        }

        private void nvMain_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                contentFrame.Navigate(typeof(SettingsPage));
            }
            else
            {
                var selectedItem = (NavigationViewItem)args.SelectedItem;

                if (selectedItem != null)
                {
                    string selectedItemTag = ((string)selectedItem.Tag);
                    string pageName = "SysAdmin.Views." + selectedItemTag;
                    Type pageType = Type.GetType(pageName);
                    contentFrame.Navigate(pageType);

                    if (selectedItemTag == "LoginPage")
                    {
                        nvMain.IsPaneVisible = false;
                        var loginPage = (LoginPage)contentFrame.Content;
                        loginPage.Connecting += LoginPage_Connecting;
                        loginPage.Connected += LoginPage_Connected;
                        loginPage.Error += LoginPage_Error;
                    }
                }
            }
        }

        private void LoginPage_Connecting(object sender, EventArgs e)
        {
            var loginPage = (LoginPage)contentFrame.Content;
            loginPage.Visibility = Visibility.Collapsed;

            progressRing.Visibility = Visibility.Visible;
        }

        private void LoginPage_Error(object sender, EventArgs e)
        {
            progressRing.Visibility = Visibility.Collapsed;
            infoBar.Show(InfoBarSeverity.Error, "Error", "Domain unavailable");

            var loginPage = (LoginPage)contentFrame.Content;
            loginPage.Visibility = Visibility.Visible;
        }

        private void LoginPage_Connected(object sender, EventArgs e)
        {
            progressRing.Visibility = Visibility.Collapsed;

            navItemLogin.IsSelected = false;
            navItemLogin.Visibility = Visibility.Collapsed;

            nvMain.IsPaneVisible = true;

            contentFrame.Navigate(typeof(HomePage));
        }

        private void nvMain_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            TryGoBack();
        }

        public bool CanGoBack()
        {
            // Remove Login Page from navigation
            if (contentFrame.CanGoBack && contentFrame.BackStack[contentFrame.BackStackDepth - 1].SourcePageType == typeof(LoginPage))
                contentFrame.BackStack.RemoveAt(contentFrame.BackStackDepth - 1);

            if (!contentFrame.CanGoBack)
                return false;

            // Don't go back if the nav pane is overlayed.
            if (nvMain.IsPaneOpen &&
                (nvMain.DisplayMode == NavigationViewDisplayMode.Compact ||
                 nvMain.DisplayMode == NavigationViewDisplayMode.Minimal))
                return false;

            return true;
        }

        private bool TryGoBack()
        {
            if (CanGoBack())
            {
                contentFrame.GoBack();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}