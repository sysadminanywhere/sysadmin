﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Sysadmin.Services;
using SysAdmin.ActiveDirectory;
using SysAdmin.ActiveDirectory.Models;
using SysAdmin.ActiveDirectory.Repositories;
using SysAdmin.ActiveDirectory.Services.Ldap;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wpf.Ui.Controls;
using Wpf.Ui;

namespace Sysadmin.ViewModels
{
    public partial class UsersViewModel : ViewModel
    {
        private bool isInitialized = false;

        private INavigationService navigationService;
        private IExchangeService exchangeService;

        [ObservableProperty]
        private IEnumerable<UserEntry> _users = new List<UserEntry>();

        [ObservableProperty]
        private bool _isBusy;

        private List<UserEntry> cache =  new List<UserEntry>();

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
            this.navigationService = navigationService;
            this.exchangeService = exchangeService;
        }

        public override async void OnNavigatedTo()
        {
            if (!isInitialized)
                InitializeViewModel();

            await ListAsync();

            SortingAndFiltering();
        }

        private void InitializeViewModel()
        {
            isInitialized = true;
        }

        [RelayCommand]
        private void OnAdd()
        {
            navigationService.Navigate(typeof(Views.Pages.AddUserPage));
        }

        [RelayCommand]
        private void OnSelectedItemsChanged(IEnumerable<object> items)
        {
            if (items.Any())
            {
                exchangeService.SetParameter((UserEntry)items.First());
                navigationService.Navigate(typeof(Views.Pages.UserPage));
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
