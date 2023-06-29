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

        private IStateService stateService;

        private MainWindowViewModel mainWindowViewModel;

        public LoginViewModel(INavigationService navigationService, IStateService stateService, MainWindowViewModel mainWindowViewModel)
        {
            _navigationService = navigationService;
            this.stateService = stateService;
            this.mainWindowViewModel = mainWindowViewModel;
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
            stateService.IsLoggedIn = true;

            mainWindowViewModel.InitializeViewModel();

            _navigationService.Navigate(typeof(Views.Pages.DashboardPage));
        }

    }
}
