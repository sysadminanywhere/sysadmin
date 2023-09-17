using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Sysadmin.Services;
using SysAdmin.ActiveDirectory;
using SysAdmin.ActiveDirectory.Models;
using SysAdmin.ActiveDirectory.Repositories;
using SysAdmin.ActiveDirectory.Services.Ldap;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        [ObservableProperty]
        private IEnumerable<UserEntry> _users;

        [ObservableProperty]
        private bool _isBusy;

        private List<UserEntry> cache;

        public enum Filters
        {
            All,
            AccountEnabled,
            AccountDisabled,
            Locked,
            PasswordExpired,
            NeverExpires
        }

        private Filters filters = Filters.All;
        private string searchText = string.Empty;
        private bool isAsc = true;

        public UsersViewModel(INavigationService navigationService, IExchangeService exchangeService)
        {
            _navigationService = navigationService;
            _exchangeService = exchangeService;
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
            _navigationService.Navigate(typeof(Views.Pages.AddUserPage));
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
                        Users = cache.OrderBy(c => c.CN);
                    }
                }
            });

            IsBusy = false;
        }

        [RelayCommand]
        private void OnSearch(string text)
        {
            searchText = text;

            SortingAndFiltering();
        }

        [RelayCommand]
        private void OnSort(MenuItem menu)
        {
            switch (menu.Tag)
            {
                case "asc":
                    isAsc = true;
                    break;
                case "desc":
                    isAsc = false;
                    break;
            }

            SortingAndFiltering();
        }

        [RelayCommand]
        private void OnFilter(MenuItem menu)
        {
            switch (menu.Tag)
            {
                case "all":
                    filters = Filters.All;
                    break;
                case "enabled":
                    filters = Filters.AccountEnabled;
                    break;
                case "disabled":
                    filters = Filters.AccountDisabled;
                    break;
                case "locked":
                    filters = Filters.Locked;
                    break;
                case "expired":
                    filters = Filters.PasswordExpired;
                    break;
                case "never_expires":
                    filters = Filters.NeverExpires;
                    break;
            }

            SortingAndFiltering();
        }

        private void SortingAndFiltering()
        {
            if (cache != null)
            {
                if (string.IsNullOrEmpty(searchText))
                {
                    Users = cache;
                }
                else
                {
                    Users = cache.Where(c => c.CN.ToUpper().StartsWith(searchText.ToUpper()));
                }

                switch (filters)
                {
                    case Filters.All:
                        Users = Users;
                        break;

                    case Filters.AccountEnabled:
                        Users = Users.Where(c => (c.UserControl & UserAccountControls.ACCOUNTDISABLE) != UserAccountControls.ACCOUNTDISABLE);
                        break;

                    case Filters.AccountDisabled:
                        Users = Users.Where(c => (c.UserControl & UserAccountControls.ACCOUNTDISABLE) == UserAccountControls.ACCOUNTDISABLE);
                        break;

                    case Filters.Locked:
                        Users = Users.Where(c => c.UserControl == SysAdmin.ActiveDirectory.UserAccountControls.LOCKOUT);
                        break;

                    case Filters.NeverExpires:
                        Users = Users.Where(c => c.UserControl == SysAdmin.ActiveDirectory.UserAccountControls.DONT_EXPIRE_PASSWD);
                        break;

                    case Filters.PasswordExpired:
                        Users = Users.Where(c => c.UserControl == SysAdmin.ActiveDirectory.UserAccountControls.PASSWORD_EXPIRED);
                        break;
                }

                if (isAsc)
                    Users = Users.OrderBy(c => c.CN);
                else
                    Users = Users.OrderByDescending(c => c.CN);
            }
        }

    }
}
