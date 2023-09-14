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

        public enum Filters
        {
            All,
            GlobalDistribution,
            DomainLocalDistribution,
            UniversalDistribution,
            GlobalSecurity,
            DomainLocalSecurity,
            UniversalSecurity,
            BuiltIn
        }

        private Filters filters = Filters.All;
        private string searchText = string.Empty;
        private bool isAsc = true;

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
                        Groups = cache.OrderBy(c => c.CN);
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
            }

            SortingAndFiltering();
        }

        private void SortingAndFiltering()
        {
            if (cache != null)
            {
                if (string.IsNullOrEmpty(searchText))
                {
                    Groups = cache;
                }
                else
                {
                    Groups = cache.Where(c => c.CN.ToUpper().StartsWith(searchText.ToUpper()));
                }

                switch (filters)
                {
                    case Filters.All:
                        Groups = Groups;
                        break;

                    case Filters.BuiltIn:
                        Groups = Groups.Where(c => c.GroupType == -2147483643);
                        break;

                    case Filters.DomainLocalDistribution:
                        Groups = Groups.Where(c => c.GroupType == 4);
                        break;

                    case Filters.DomainLocalSecurity:
                        Groups = Groups.Where(c => c.GroupType == -2147483644);
                        break;

                    case Filters.GlobalDistribution:
                        Groups = Groups.Where(c => c.GroupType == 2);
                        break;

                    case Filters.GlobalSecurity:
                        Groups = Groups.Where(c => c.GroupType == -2147483646);
                        break;

                    case Filters.UniversalDistribution:
                        Groups = Groups.Where(c => c.GroupType == 8);
                        break;

                    case Filters.UniversalSecurity:
                        Groups = Groups.Where(c => c.GroupType == -2147483640);
                        break;
                }

                if (isAsc)
                    Groups = Groups.OrderBy(c => c.CN);
                else
                    Groups = Groups.OrderByDescending(c => c.CN);
            }
        }

    }
}