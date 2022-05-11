using LdapForNet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using SysAdmin.ActiveDirectory;
using SysAdmin.ActiveDirectory.Models;
using SysAdmin.ActiveDirectory.Repositories;
using SysAdmin.ActiveDirectory.Services.Ldap;
using SysAdmin.Services;
using SysAdmin.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SysAdmin.ViewModels
{
    public class ComputersViewModel : ObservableObject
    {
        public enum Filters
        {
            All,
            AccountEnabled,
            AccountDisabled
        }

        public ComputerEntry Computer { get; set; } = new ComputerEntry();
        public ObservableCollection<ComputerEntry> Computers { get; private set; } = new ObservableCollection<ComputerEntry>();

        public RelayCommand<object> AddCommand { get; private set; }
        public RelayCommand<object> ModifyCommand { get; private set; }
        public RelayCommand<object> DeleteCommand { get; private set; }
        public RelayCommand<string> SearchCommand { get; private set; }
        public RelayCommand<Filters> FilterCommand { get; private set; }

        public RelayCommand<object> SortAscCommand { get; private set; }
        public RelayCommand<object> SortDescCommand { get; private set; }

        INavigationService navigation = App.Current.Services.GetService<INavigationService>();
        INotificationService notification = App.Current.Services.GetService<INotificationService>();
        IBusyService busyService = App.Current.Services.GetService<IBusyService>();

        private List<ComputerEntry> cache;

        private Filters filters = Filters.All;
        private string searchText = string.Empty;
        private bool isAsc = true;

        public ComputersViewModel()
        {
            AddCommand = new RelayCommand<object>((xaml) => AddComputer(xaml));
            ModifyCommand = new RelayCommand<object>((xaml) => ModifyComputer(xaml));
            DeleteCommand = new RelayCommand<object>((xaml) => DeleteComputer(xaml));
            SearchCommand = new RelayCommand<string>((text) => SearchComputers(text));
            FilterCommand = new RelayCommand<Filters>((filter) => FilterComputers(filter));

            SortAscCommand = new RelayCommand<object>((xaml) => SortAsc());
            SortDescCommand = new RelayCommand<object>((xaml) => SortDesc());
        }

        private void FilterComputers(Filters filter)
        {
            filters = filter;
            SortingAndFiltering();
        }

        private void SearchComputers(string text)
        {
            searchText = text;
            SortingAndFiltering();
        }

        private void SortDesc()
        {
            isAsc = false;
            SortingAndFiltering();
        }

        private void SortAsc()
        {
            isAsc = true;
            SortingAndFiltering();
        }

        private void SortingAndFiltering()
        {
            if (string.IsNullOrEmpty(searchText))
            {
                Computers = new ObservableCollection<ComputerEntry>(cache);
            }
            else
            {
                Computers = new ObservableCollection<ComputerEntry>(cache.Where(c => c.CN.ToUpper().StartsWith(searchText.ToUpper())));
            }

            switch (filters)
            {
                case Filters.All:
                    Computers = new ObservableCollection<ComputerEntry>(Computers);
                    break;

                case Filters.AccountEnabled:
                    Computers = new ObservableCollection<ComputerEntry>(Computers.Where(c => (c.UserControl & UserAccountControls.ACCOUNTDISABLE) != UserAccountControls.ACCOUNTDISABLE));
                    break;

                case Filters.AccountDisabled:
                    Computers = new ObservableCollection<ComputerEntry>(Computers.Where(c => (c.UserControl & UserAccountControls.ACCOUNTDISABLE) == UserAccountControls.ACCOUNTDISABLE));
                    break;
            }

            if (isAsc)
                Computers = new ObservableCollection<ComputerEntry>(Computers.OrderBy(c => c.CN));
            else
                Computers = new ObservableCollection<ComputerEntry>(Computers.OrderByDescending(c => c.CN));

            OnPropertyChanged(nameof(Computers));
        }

        private async void AddComputer(object xaml)
        {
            IAddComputerDialogService dialog = App.Current.Services.GetService<IAddComputerDialogService>();
            var result = await dialog.ShowDialog(await GetDefaultContainer(), xaml);
            if (result == true)
            {
                busyService.Busy();

                try
                {
                    await Add(dialog.DistinguishedName, dialog.Computer, dialog.IsAccountEnabled);
                    notification.ShowSuccessMessage("Computer added");
                    await ListAsync();
                }
                catch (LdapException le)
                {
                    notification.ShowErrorMessage(ActiveDirectory.LdapResult.GetErrorMessageFromResult(le.ResultCode));
                }
                catch (Exception ex)
                {
                    notification.ShowErrorMessage(ex.Message);
                }
            }

            busyService.Idle();
        }

        private async void ModifyComputer(object xaml)
        {
            IEditComputerDialogService dialog = App.Current.Services.GetService<IEditComputerDialogService>();
            dialog.Computer = Computer;
            var result = await dialog.ShowDialog(xaml);
            if (result == true)
            {
                busyService.Idle();

                try
                {
                    Computer = await Modify(dialog.Computer);
                    notification.ShowSuccessMessage("Computer modified");
                    OnPropertyChanged(nameof(Computer));
                }
                catch (LdapException le)
                {
                    notification.ShowErrorMessage(ActiveDirectory.LdapResult.GetErrorMessageFromResult(le.ResultCode));
                }
                catch (Exception ex)
                {
                    notification.ShowErrorMessage(ex.Message);
                }
            }

            busyService.Idle();
        }

        private async void DeleteComputer(object xaml)
        {
            IQuestionDialogService dialog = App.Current.Services.GetService<IQuestionDialogService>();
            var result = await dialog.ShowDialog(xaml, "Delete", "Are you sure you want to delete this computer?");
            if (result == true)
            {
                busyService.Busy();

                try
                {
                    await Delete(Computer);
                    notification.ShowSuccessMessage("Computer deleted");
                    if (navigation.CanGoBack) navigation.GoBack();
                }
                catch (LdapException le)
                {
                    notification.ShowErrorMessage(ActiveDirectory.LdapResult.GetErrorMessageFromResult(le.ResultCode));
                }
                catch (Exception ex)
                {
                    notification.ShowErrorMessage(ex.Message);
                }
            }

            busyService.Idle();
        }

        public async Task ListAsync()
        {
            busyService.Busy();

            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    using (var computersRepository = new ComputersRepository(ldap))
                    {
                        cache = await computersRepository.ListAsync();
                        if (cache == null)
                            cache = new List<ComputerEntry>();
                        Computers = new ObservableCollection<ComputerEntry>(cache);
                    }
                }
            });
            OnPropertyChanged(nameof(Computers));

            busyService.Idle();
        }

        public async Task Add(string distinguishedName, ComputerEntry computer, bool isEnabled)
        {
            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    using (var computersRepository = new ComputersRepository(ldap))
                    {
                        await computersRepository.AddAsync(distinguishedName, computer, isEnabled);
                    }
                }
            });
        }

        public async Task<ComputerEntry> Modify(ComputerEntry computer)
        {
            ComputerEntry entry = null;

            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    using (var computersRepository = new ComputersRepository(ldap))
                    {
                        entry = await computersRepository.ModifyAsync(computer);
                    }
                }
            });

            return entry;
        }

        public async Task Delete(ComputerEntry computer)
        {
            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    using (var computersRepository = new ComputersRepository(ldap))
                    {
                        await computersRepository.DeleteAsync(computer);
                    }
                }
            });
        }

        public async Task Get(string cn)
        {
            busyService.Busy();

            ComputerEntry entry = Computer;

            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    using (var computersRepository = new ComputersRepository(ldap))
                    {
                        entry = await computersRepository.GetByCNAsync(cn);
                    }
                }
            });

            Computer = entry;
            OnPropertyChanged(nameof(Computer));

            busyService.Idle();
        }

        private async Task<string> GetDefaultContainer()
        {
            string item = string.Empty;

            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    var resultWK = await ldap.WellKnownObjectsAsync();
                    item = resultWK.Where(c => c.StartsWith(ADContainers.ContainerComputers)).First();
                }
            });

            return item.Replace(ADContainers.ContainerComputers, string.Empty);
        }

    }
}