using Microsoft.Extensions.DependencyInjection;
using SysAdmin.Services;
using System;
using System.Windows;
using System.Windows.Controls;
using AutoUpdaterDotNET;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.IO;
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

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

        public MainWindow(ViewModels.MainWindowViewModel viewModel, 
            IPageService pageService, 
            INavigationService navigationService, 
            IServiceProvider serviceProvider,
            ISnackbarService snackbarService,
            IContentDialogService contentDialogService)
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
            snackbarService.SetSnackbarPresenter(SnackbarPresenter);
            contentDialogService.SetDialogHost(RootContentDialog);
        }

        private void SetTheme()
        {

            switch (settings.ThemeSetting)
            {
                case "theme_light":
                    if (ApplicationThemeManager.GetAppTheme() == ApplicationTheme.Light)
                        break;

                    ApplicationThemeManager.Apply(ApplicationTheme.Light);

                    break;

                default:
                    if (ApplicationThemeManager.GetAppTheme() == ApplicationTheme.Dark)
                        break;

                    ApplicationThemeManager.Apply(ApplicationTheme.Dark);

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



        #region INavigationWindow methods

        public INavigationView GetNavigation() => RootNavigation;

        public bool Navigate(Type pageType) => RootNavigation.Navigate(pageType);

        public void SetPageService(IPageService pageService) => RootNavigation.SetPageService(pageService);

        public void ShowWindow() => Show();

        public void CloseWindow() => Close();

        #endregion INavigationWindow methods

        /// <summary>
        /// Raises the closed event.
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Make sure that closing this window will begin the process of closing the application.
            Application.Current.Shutdown();
        }

        INavigationView INavigationWindow.GetNavigation()
        {
            throw new NotImplementedException();
        }

        public void SetServiceProvider(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }
    }
}