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
    public partial class PrintersViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        private INavigationService _navigationService;
        private IExchangeService _exchangeService;

        [ObservableProperty]
        private IEnumerable<PrinterEntry> _printers;

        private List<PrinterEntry> cache;

        [ObservableProperty]
        private bool _isBusy;

        private string searchText = string.Empty;
        private bool isAsc = true;

        public PrintersViewModel(INavigationService navigationService, IExchangeService exchangeService)
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

        }

        [RelayCommand]
        private void OnSelectedItemsChanged(IEnumerable<object> items)
        {
            if (items != null && items.Count() > 0)
            {
                _exchangeService.SetParameter((PrinterEntry)items.First());
                _navigationService.Navigate(typeof(Views.Pages.PrinterPage));
            }
        }

        public async Task ListAsync()
        {

            IsBusy = true;

            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    using (var printersRepository = new PrintersRepository(ldap))
                    {
                        cache = await printersRepository.ListAsync();
                        if (cache == null)
                            cache = new List<PrinterEntry>();
                        Printers = cache.OrderBy(c => c.CN);
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

        private void SortingAndFiltering()
        {
            if (cache != null)
            {
                if (string.IsNullOrEmpty(searchText))
                {
                    Printers = cache;
                }
                else
                {
                    Printers = cache.Where(c => c.CN.ToUpper().StartsWith(searchText.ToUpper()));
                }

                if (isAsc)
                    Printers = Printers.OrderBy(c => c.CN);
                else
                    Printers = Printers.OrderByDescending(c => c.CN);
            }
        }

    }
}