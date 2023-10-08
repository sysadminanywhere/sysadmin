using Microsoft.Extensions.DependencyInjection;
using SysAdmin.Services;
using System;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using AutoUpdaterDotNET;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.IO;
using System.Windows.Media;
using System.Runtime.InteropServices;

namespace Sysadmin.Views.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INavigationWindow
    {

        ISettingsService settings;

        public ViewModels.MainWindowViewModel ViewModel
        {
            get;
        }

        public MainWindow(ViewModels.MainWindowViewModel viewModel, IPageService pageService, INavigationService navigationService, IServiceProvider serviceProvider)
        {

            settings = serviceProvider.GetService<ISettingsService>();
            settings.LoadSettings();

            SetTheme();

            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
            SetPageService(pageService);

            AutoUpdater.UpdateFormSize = new System.Drawing.Size(450, 200);
            AutoUpdater.Icon = BitmapImage2Bitmap(new BitmapImage(new Uri("pack://application:,,,/Assets/StoreLogo.bmp", UriKind.RelativeOrAbsolute)));
            AutoUpdater.Start("https://raw.githubusercontent.com/sysadminanywhere/sysadmin/main/src/autoupdater.xml");

            navigationService.SetNavigationControl(RootNavigation);
        }

        #region INavigationWindow methods

        public Frame GetFrame()
            => RootFrame;

        public INavigation GetNavigation()
            => RootNavigation;

        public bool Navigate(Type pageType)
            => RootNavigation.Navigate(pageType);

        public void SetPageService(IPageService pageService)
            => RootNavigation.PageService = pageService;

        public void ShowWindow()
            => Show();

        public void CloseWindow()
            => Close();

        #endregion INavigationWindow methods

        /// <summary>
        /// Raises the closed event.
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            settings.SaveSettings();

            // Make sure that closing this window will begin the process of closing the application.
            Application.Current.Shutdown();
        }

        private void SetTheme() 
        {

            switch (settings.ThemeSetting)
            {
                case "theme_light":
                    if (Wpf.Ui.Appearance.Theme.GetAppTheme() == Wpf.Ui.Appearance.ThemeType.Light)
                        break;

                    Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Light);

                    break;

                default:
                    if (Wpf.Ui.Appearance.Theme.GetAppTheme() == Wpf.Ui.Appearance.ThemeType.Dark)
                        break;

                    Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Dark);

                    break;
            }

        }

        private Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }

    }
}