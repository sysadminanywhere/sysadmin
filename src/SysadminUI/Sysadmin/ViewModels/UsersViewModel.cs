using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Sysadmin.Services;
using SysAdmin.ActiveDirectory.Models;
using SysAdmin.ActiveDirectory.Repositories;
using SysAdmin.ActiveDirectory.Services.Ldap;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Controls;
using Wpf.Ui.Mvvm.Contracts;

namespace Sysadmin.ViewModels
{
    public partial class UsersViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        private INavigationService _navigationService;
        private IExchangeService _exchangeService;
        private IWindowService _windowService;

        [ObservableProperty]
        private IEnumerable<UserEntry> _users;

        [ObservableProperty]
        private bool _isBusy;

        private List<UserEntry> cache;

        public UsersViewModel(INavigationService navigationService, IExchangeService exchangeService, IWindowService windowService)
        {
            _navigationService = navigationService;
            _exchangeService = exchangeService;
            _windowService = windowService;
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
            _windowService.AddUserWindow();
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
                _exchangeService.SetParameter((UserEntry)items.First());
                _navigationService.Navigate(typeof(Views.Pages.UserPage));
            }
        }

        public async Task ListAsync()
        {

            IsBusy = true;

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

            IsBusy = false;
        }


    }
}
