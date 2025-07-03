using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SysAdmin.Services;
using System;
using Wpf.Ui.Appearance;

namespace Sysadmin.ViewModels
{
    public partial class SettingsViewModel : ViewModel
    {
        private bool isInitialized = false;

        private ISettingsService settings;

        [ObservableProperty]
        private string _appVersion = String.Empty;

        [ObservableProperty]
        private ApplicationTheme _currentTheme = ApplicationTheme.Unknown;

        public SettingsViewModel(ISettingsService settings) 
        {
            this.settings = settings;
        }

        public override void OnNavigatedTo()
        {
            if (!isInitialized)
                InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            CurrentTheme = ApplicationThemeManager.GetAppTheme();
            AppVersion = $"Version: {GetAssemblyVersion()}";

            isInitialized = true;
        }

        private string GetAssemblyVersion()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? String.Empty;
        }

        [RelayCommand]
        private void OnChangeTheme(string parameter)
        {
            //switch (parameter)
            //{
            //    case "theme_light":
            //        if (CurrentTheme == Wpf.Ui.Appearance.ThemeType.Light)
            //            break;

            //        Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Light);
            //        CurrentTheme = Wpf.Ui.Appearance.ThemeType.Light;

            //        break;

            //    default:
            //        if (CurrentTheme == Wpf.Ui.Appearance.ThemeType.Dark)
            //            break;

            //        Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Dark);
            //        CurrentTheme = Wpf.Ui.Appearance.ThemeType.Dark;

            //        break;
            //}

            //settings.ThemeSetting = parameter;
        }
    }
}
