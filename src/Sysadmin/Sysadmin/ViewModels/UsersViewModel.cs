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
using Windows.Storage;

namespace SysAdmin.ViewModels
{
    public class UsersViewModel : ObservableObject
    {

        public enum Filters
        {
            All,
            AccountEnabled,
            AccountDisabled,
            Locked,
            PasswordExpired,
            NeverExpires
        }

        public UserEntry User { get; set; } = new UserEntry();
        public ObservableCollection<UserEntry> Users { get; private set; } = new ObservableCollection<UserEntry>();

        public RelayCommand AddCommand { get; private set; }
        public RelayCommand ResetPasswordCommand { get; private set; }
        public RelayCommand ModifyCommand { get; private set; }
        public RelayCommand DeleteCommand { get; private set; }
        public RelayCommand OptionsCommand { get; private set; }
        public RelayCommand<string> SearchCommand { get; private set; }
        public RelayCommand<Filters> FilterCommand { get; private set; }

        INavigationService navigation = App.Current.Services.GetService<INavigationService>();
        INotificationService notification = App.Current.Services.GetService<INotificationService>();
        IBusyService busyService = App.Current.Services.GetService<IBusyService>();
        ISettingsService settings = App.Current.Services.GetService<ISettingsService>();

        private List<UserEntry> cache;

        private Filters filters = Filters.All;
        private string searchText = string.Empty;
        private bool isAsc = true;

        public RelayCommand SortAscCommand { get; private set; }
        public RelayCommand SortDescCommand { get; private set; }

        public UsersViewModel()
        {
            AddCommand = new RelayCommand(() => AddUser());
            ResetPasswordCommand = new RelayCommand(() => ResetUserPassword());
            ModifyCommand = new RelayCommand(() => ModifyUser());
            DeleteCommand = new RelayCommand(() => DeleteUser());
            OptionsCommand = new RelayCommand(() => UserOptions());
            SearchCommand = new RelayCommand<string>((text) => SearchUsers(text));
            FilterCommand = new RelayCommand<Filters>((filter) => FilterUsers(filter));

            SortAscCommand = new RelayCommand(() => SortAsc());
            SortDescCommand = new RelayCommand(() => SortDesc());
        }

        private void FilterUsers(Filters filter)
        {
            filters = filter;
            SortingAndFiltering();
        }

        private void SearchUsers(string text)
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
                Users = new ObservableCollection<UserEntry>(cache);
            }
            else
            {
                Users = new ObservableCollection<UserEntry>(cache.Where(c => c.CN.ToUpper().StartsWith(searchText.ToUpper())));
            }

            switch (filters)
            {
                case Filters.All:
                    Users = new ObservableCollection<UserEntry>(Users);
                    break;

                case Filters.AccountEnabled:
                    Users = new ObservableCollection<UserEntry>(Users.Where(c => (c.UserControl & UserAccountControls.ACCOUNTDISABLE) != UserAccountControls.ACCOUNTDISABLE));
                    break;

                case Filters.AccountDisabled:
                    Users = new ObservableCollection<UserEntry>(Users.Where(c => (c.UserControl & UserAccountControls.ACCOUNTDISABLE) == UserAccountControls.ACCOUNTDISABLE));
                    break;

                case Filters.Locked:
                    Users = new ObservableCollection<UserEntry>(Users.Where(c => c.UserControl == ActiveDirectory.UserAccountControls.LOCKOUT));
                    break;

                case Filters.NeverExpires:
                    Users = new ObservableCollection<UserEntry>(Users.Where(c => c.UserControl == ActiveDirectory.UserAccountControls.DONT_EXPIRE_PASSWD));
                    break;

                case Filters.PasswordExpired:
                    Users = new ObservableCollection<UserEntry>(Users.Where(c => c.UserControl == ActiveDirectory.UserAccountControls.PASSWORD_EXPIRED));
                    break;
            }

            if (isAsc)
                Users = new ObservableCollection<UserEntry>(Users.OrderBy(c => c.CN));
            else
                Users = new ObservableCollection<UserEntry>(Users.OrderByDescending(c => c.CN));

            OnPropertyChanged(nameof(Users));
        }

        private async void AddUser()
        {
            IAddUserDialogService dialog = App.Current.Services.GetService<IAddUserDialogService>();
            var result = await dialog.ShowDialog(await GetDefaultContainer());
            if (result == true)
            {
                busyService.Busy();

                try
                {
                    await Add(dialog.DistinguishedName, dialog.User, dialog.Password);
                    notification.ShowSuccessMessage("User added");
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

        private async void ResetUserPassword()
        {
            IResetPasswordDialog dialog = App.Current.Services.GetService<IResetPasswordDialog>();

            dialog.Password = settings.UserDefaultPassword;

            var result = await dialog.ShowDialog();
            if (result == true)
            {
                busyService.Busy();

                try
                {
                    await ResetPassword(dialog.User, dialog.Password);
                    notification.ShowSuccessMessage("Password changed");
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

        private async void ModifyUser()
        {
            IEditUserDialogService dialog = App.Current.Services.GetService<IEditUserDialogService>();
            dialog.User = User;
            var result = await dialog.ShowDialog();
            if (result == true)
            {
                busyService.Busy();

                try
                {
                    User = await Modify(dialog.User);
                    notification.ShowSuccessMessage("User modified");
                    OnPropertyChanged(nameof(User));
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

        private async void DeleteUser()
        {
            IQuestionDialogService dialog = App.Current.Services.GetService<IQuestionDialogService>();
            var result = await dialog.ShowDialog("Delete", "Are you sure you want to delete this user?");
            if (result == true)
            {
                busyService.Busy();

                try
                {
                    await Delete(User);
                    notification.ShowSuccessMessage("User deleted");
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

        private async void UserOptions()
        {
            IUserOptionsDialogService dialog = App.Current.Services.GetService<IUserOptionsDialogService>();
            dialog.User = User;
            var result = await dialog.ShowDialog();
            if (result == true)
            {
                busyService.Busy();

                try
                {
                    User = await Modify(dialog.User);
                    notification.ShowSuccessMessage("User options modified");
                    OnPropertyChanged(nameof(User));
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
                    using (var usersRepository = new UsersRepository(ldap))
                    {
                        cache = await usersRepository.ListAsync();
                        if (cache == null)
                            cache = new List<UserEntry>();
                        Users = new ObservableCollection<UserEntry>(cache);
                    }
                }
            });
            OnPropertyChanged(nameof(Users));

            busyService.Idle();
        }

        public async Task Add(string distinguishedName, UserEntry user, string password)
        {
            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    using (var usersRepository = new UsersRepository(ldap))
                    {
                        if (string.IsNullOrEmpty(user.CN))
                            user.CN = user.DisplayName;
                        await usersRepository.AddAsync(distinguishedName, user, password);
                    }
                }
            });
        }

        public async Task<UserEntry> Modify(UserEntry user)
        {
            UserEntry entry = null;

            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    using (var usersRepository = new UsersRepository(ldap))
                    {
                        entry = await usersRepository.ModifyAsync(user);
                    }
                }
            });

            return entry;
        }

        public async Task Delete(UserEntry user)
        {
            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    using (var usersRepository = new UsersRepository(ldap))
                    {
                        await usersRepository.DeleteAsync(user);
                    }
                }
            });
        }

        public async Task Get(string cn)
        {
            busyService.Busy();

            UserEntry entry = User;

            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    using (var usersRepository = new UsersRepository(ldap))
                    {
                        entry = await usersRepository.GetByCNAsync(cn);
                    }
                }
            });

            User = entry;
            OnPropertyChanged(nameof(User));

            busyService.Idle();
        }

        public async Task ResetPassword(UserEntry user, string password)
        {
            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    using (var usersRepository = new UsersRepository(ldap))
                    {
                        if (string.IsNullOrEmpty(user.CN))
                            user.CN = user.DisplayName;
                        await usersRepository.ResetPasswordAsync(user, password);
                        await usersRepository.MustChangePasswordAsync(user);
                    }
                }
            });
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