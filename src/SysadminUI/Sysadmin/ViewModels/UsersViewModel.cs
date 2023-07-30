using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Sysadmin.Messages;
using SysAdmin.ActiveDirectory.Models;
using SysAdmin.ActiveDirectory.Repositories;
using SysAdmin.ActiveDirectory.Services.Ldap;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace Sysadmin.ViewModels
{
    public partial class UsersViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        private INavigationService _navigationService;

        [ObservableProperty]
        private IEnumerable<UserEntry> _users;

        private List<UserEntry> cache;

        public UsersViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public async void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();

            await ListAsync();
        }

        public void OnNavigatedFrom()
        {
        }

        private void InitializeViewModel()
        {
            _isInitialized = true;
        }

        [RelayCommand]
        private void OnAdd()
        {

        }

        [RelayCommand]
        private void OnSort(MenuItem menu)
        {
            switch (menu.Tag)
            {
                case "asc":
                    break;
                case "desc":
                    break;
            }
        }

        [RelayCommand]
        private void OnFilter(MenuItem menu)
        {
            switch (menu.Tag)
            {
                case "all":
                    break;
                case "enabled":
                    break;
                case "disabled":
                    break;
                case "locked":
                    break;
                case "expired":
                    break;
                case "never_expires":
                    break;
            }
        }

        [RelayCommand]
        private void OnSelectedItemsChanged(IEnumerable<object> items)
        {
            if (items != null && items.Count() > 0)
            {
                UserEntry user = (UserEntry)items.First();
                _navigationService.Navigate(typeof(Views.Pages.UserPage));
                WeakReferenceMessenger.Default.Send(new UserSelectededMessage(user));
            }
        }

        public async Task ListAsync()
        {
            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    using (var usersRepository = new UsersRepository(ldap))
                    {
                        cache = await usersRepository.ListAsync();
                        if (cache == null)
                            cache = new List<UserEntry>();
                        Users = cache;
                    }
                }
            });
        }


    }
}
