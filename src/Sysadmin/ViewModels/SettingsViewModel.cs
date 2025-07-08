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
            switch (parameter)
            {
                case "theme_light":
                    if (CurrentTheme == ApplicationTheme.Light)
                        break;

                    ApplicationThemeManager.Apply(ApplicationTheme.Light);
                    CurrentTheme = ApplicationTheme.Light;

                    break;

                default:
                    if (CurrentTheme == ApplicationTheme.Dark)
                        break;

                    ApplicationThemeManager.Apply(ApplicationTheme.Dark);
                    CurrentTheme = ApplicationTheme.Dark;

                    break;
            }

            settings.ThemeSetting = parameter;
        }
    }
}
