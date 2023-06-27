using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Sysadmin.Models;
using Sysadmin.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace Sysadmin.ViewModels
{
    public partial class LoginViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        private INavigationService _navigationService;

        private IStateService _stateService;

        public LoginViewModel(INavigationService navigationService, IServiceProvider serviceProvider)
        {
            _navigationService = navigationService;
            _stateService = serviceProvider.GetService<IStateService>();
        }

        public void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        public void OnNavigatedFrom()
        {
        }

        private void InitializeViewModel()
        {
            _isInitialized = true;
        }

        [RelayCommand]
        private void OnLogin()
        {
            _stateService.IsLoggedIn = true;

            Views.Windows.MainWindow mainWindow = Application.Current.Windows.OfType<Views.Windows.MainWindow>().First();
            mainWindow.InitializeComponent();

            _navigationService.Navigate(typeof(Views.Pages.DashboardPage));
        }

    }
}
