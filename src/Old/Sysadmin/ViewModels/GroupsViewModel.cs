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
    public class GroupsViewModel : ObservableObject
    {
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

        public GroupEntry Group { get; set; } = new GroupEntry();
        public ObservableCollection<GroupEntry> Groups { get; private set; } = new ObservableCollection<GroupEntry>();

        public RelayCommand AddCommand { get; private set; }
        public RelayCommand ModifyCommand { get; private set; }
        public RelayCommand DeleteCommand { get; private set; }
        public RelayCommand<string> SearchCommand { get; private set; }
        public RelayCommand<Filters> FilterCommand { get; private set; }

        public RelayCommand SortAscCommand { get; private set; }
        public RelayCommand SortDescCommand { get; private set; }

        INavigationService navigation = App.Current.Services.GetService<INavigationService>();
        INotificationService notification = App.Current.Services.GetService<INotificationService>();
        IBusyService busyService = App.Current.Services.GetService<IBusyService>();

        private List<GroupEntry> cache;

        private Filters filters = Filters.All;
        private string searchText = string.Empty;
        private bool isAsc = true;

        public GroupsViewModel()
        {
            AddCommand = new RelayCommand(() => AddGroup());
            ModifyCommand = new RelayCommand(() => ModifyGroup());
            DeleteCommand = new RelayCommand(() => DeleteGroup());
            SearchCommand = new RelayCommand<string>((text) => SearchGroups(text));
            FilterCommand = new RelayCommand<Filters>((filter) => FilterGroups(filter));

            SortAscCommand = new RelayCommand(() => SortAsc());
            SortDescCommand = new RelayCommand(() => SortDesc());
        }

        private void FilterGroups(Filters filter)
        {
            filters = filter;
            SortingAndFiltering();
        }

        private void SearchGroups(string text)
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
            if (cache != null)
            {
                if (string.IsNullOrEmpty(searchText))
                {
                    Groups = new ObservableCollection<GroupEntry>(cache);
                }
                else
                {
                    Groups = new ObservableCollection<GroupEntry>(cache.Where(c => c.CN.ToUpper().StartsWith(searchText.ToUpper())));
                }

                switch (filters)
                {
                    case Filters.All:
                        Groups = new ObservableCollection<GroupEntry>(Groups);
                        break;

                    case Filters.BuiltIn:
                        Groups = new ObservableCollection<GroupEntry>(Groups.Where(c => c.GroupType == -2147483643));
                        break;

                    case Filters.DomainLocalDistribution:
                        Groups = new ObservableCollection<GroupEntry>(Groups.Where(c => c.GroupType == 4));
                        break;

                    case Filters.DomainLocalSecurity:
                        Groups = new ObservableCollection<GroupEntry>(Groups.Where(c => c.GroupType == -2147483644));
                        break;

                    case Filters.GlobalDistribution:
                        Groups = new ObservableCollection<GroupEntry>(Groups.Where(c => c.GroupType == 2));
                        break;

                    case Filters.GlobalSecurity:
                        Groups = new ObservableCollection<GroupEntry>(Groups.Where(c => c.GroupType == -2147483646));
                        break;

                    case Filters.UniversalDistribution:
                        Groups = new ObservableCollection<GroupEntry>(Groups.Where(c => c.GroupType == 8));
                        break;

                    case Filters.UniversalSecurity:
                        Groups = new ObservableCollection<GroupEntry>(Groups.Where(c => c.GroupType == -2147483640));
                        break;
                }

                if (isAsc)
                    Groups = new ObservableCollection<GroupEntry>(Groups.OrderBy(c => c.CN));
                else
                    Groups = new ObservableCollection<GroupEntry>(Groups.OrderByDescending(c => c.CN));

                OnPropertyChanged(nameof(Groups));
            }
        }

        private async void AddGroup()
        {
            IAddGroupDialogService dialog = App.Current.Services.GetService<IAddGroupDialogService>();
            var result = await dialog.ShowDialog(await GetDefaultContainer());
            if (result == true)
            {
                busyService.Busy();

                try
                {
                    await Add(dialog.DistinguishedName, dialog.Group, dialog.GroupScope, dialog.IsSecurity);
                    notification.ShowSuccessMessage("Group added");
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

        private async void ModifyGroup()
        {
            IEditGroupDialogService dialog = App.Current.Services.GetService<IEditGroupDialogService>();
            dialog.Group = Group;
            var result = await dialog.ShowDialog();
            if (result == true)
            {
                busyService.Busy();

                try
                {
                    Group = await Modify(dialog.Group);
                    notification.ShowSuccessMessage("Group modified");
                    OnPropertyChanged(nameof(Group));
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

        private async void DeleteGroup()
        {
            IQuestionDialogService dialog = App.Current.Services.GetService<IQuestionDialogService>();
            var result = await dialog.ShowDialog("Delete", "Are you sure you want to delete this group?");
            if (result == true)
            {
                busyService.Busy();

                try
                {
                    await Delete(Group);
                    notification.ShowSuccessMessage("Group deleted");
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
                    using (var groupsRepository = new GroupsRepository(ldap))
                    {
                        cache = await groupsRepository.ListAsync();
                        if (cache == null)
                            cache = new List<GroupEntry>();
                        Groups = new ObservableCollection<GroupEntry>(cache);
                    }
                }
            });
            OnPropertyChanged(nameof(Groups));

            busyService.Idle();
        }

        public async Task Add(string distinguishedName, GroupEntry group, GroupScopes groupScope, bool isSecurity)
        {
            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    using (var groupsRepository = new GroupsRepository(ldap))
                    {
                        await groupsRepository.AddAsync(distinguishedName, group, groupScope, isSecurity);
                    }
                }
            });
        }

        public async Task<GroupEntry> Modify(GroupEntry group)
        {
            GroupEntry entry = null;

            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    using (var groupsRepository = new GroupsRepository(ldap))
                    {
                        entry = await groupsRepository.ModifyAsync(group);
                    }
                }
            });

            return entry;
        }

        public async Task Delete(GroupEntry group)
        {
            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    using (var groupsRepository = new GroupsRepository(ldap))
                    {
                        await groupsRepository.DeleteAsync(group);
                    }
                }
            });
        }

        public async Task Get(string cn)
        {
            busyService.Busy();

            GroupEntry entry = Group;

            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    using (var groupsRepository = new GroupsRepository(ldap))
                    {
                        entry = await groupsRepository.GetByCNAsync(cn);
                    }
                }
            });

            Group = entry;
            OnPropertyChanged(nameof(Group));

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
                    item = resultWK.Where(c => c.StartsWith(ADContainers.ContainerUsers)).First();
                }
            });

            return item.Replace(ADContainers.ContainerUsers, string.Empty);
        }

    }
}