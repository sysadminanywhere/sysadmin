using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Sysadmin.Messages;
using Sysadmin.Models;
using SysAdmin.ActiveDirectory.Models;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace Sysadmin.ViewModels
{
    public partial class UserViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        private INavigationService _navigationService;

        [ObservableProperty]
        private UserEntry _user;

        public UserViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();

            WeakReferenceMessenger.Default.Register<UserSelectededMessage>(this, (r, m) =>
            {
                User = m.Value;
            });
        }

        public void OnNavigatedFrom()
        {
            WeakReferenceMessenger.Default.Unregister<UserSelectededMessage>(this);
        }

        private void InitializeViewModel()
        {
            _isInitialized = true;
        }

        [RelayCommand]
        private void OnClose()
        {
            _navigationService.Navigate(typeof(Views.Pages.UsersPage));
        }

    }
}
