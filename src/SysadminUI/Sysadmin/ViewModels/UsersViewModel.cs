using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SysAdmin.ActiveDirectory.Models;
using SysAdmin.ActiveDirectory.Repositories;
using SysAdmin.ActiveDirectory.Services.Ldap;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;
using Wpf.Ui.Common.Interfaces;

namespace Sysadmin.ViewModels
{
    public partial class UsersViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;


        [ObservableProperty]
        private IEnumerable<UserEntry> _users;

        private List<UserEntry> cache;

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
