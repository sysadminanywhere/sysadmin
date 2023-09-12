﻿using CommunityToolkit.Mvvm.ComponentModel;
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
    public partial class GroupsViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        private INavigationService _navigationService;
        private IExchangeService _exchangeService;

        [ObservableProperty]
        private IEnumerable<GroupEntry> _groups;

        private List<GroupEntry> cache;

        [ObservableProperty]
        private bool _isBusy;

        public GroupsViewModel(INavigationService navigationService, IExchangeService exchangeService)
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
            _navigationService.Navigate(typeof(Views.Pages.AddGroupPage));
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
                _exchangeService.SetParameter((GroupEntry)items.First());
                _navigationService.Navigate(typeof(Views.Pages.GroupPage));
            }
        }

        public async Task ListAsync()
        {

            IsBusy = true;

            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    using (var groupsRepository = new GroupsRepository(ldap))
                    {
                        cache = await groupsRepository.ListAsync();
                        if (cache == null)
                            cache = new List<GroupEntry>();
                        Groups = cache;
                    }
                }
            });

            IsBusy = false;
        }


    }
}