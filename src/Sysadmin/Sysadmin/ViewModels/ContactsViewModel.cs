﻿using LdapForNet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
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
    public class ContactsViewModel : ObservableObject
    {

        public ContactEntry Contact { get; set; } = new ContactEntry();
        public ObservableCollection<ContactEntry> Contacts { get; private set; } = new ObservableCollection<ContactEntry>();
        
        public RelayCommand<object> AddCommand { get; private set; }
        public RelayCommand<object> ModifyCommand { get; private set; }
        public RelayCommand<object> DeleteCommand { get; private set; }
        public RelayCommand<string> SearchCommand { get; private set; }

        public RelayCommand<object> SortAscCommand { get; private set; }
        public RelayCommand<object> SortDescCommand { get; private set; }

        INavigationService navigation = App.Current.Services.GetService<INavigationService>();
        INotificationService notification = App.Current.Services.GetService<INotificationService>();
        IBusyService busyService = App.Current.Services.GetService<IBusyService>();

        private List<ContactEntry> cache;

        private string searchText = string.Empty;
        private bool isAsc = true;

        public ContactsViewModel()
        {
            AddCommand = new RelayCommand<object>((xaml) => AddContact(xaml));
            ModifyCommand = new RelayCommand<object>((xaml) => ModifyContact(xaml));
            DeleteCommand = new RelayCommand<object>((xaml) => DeleteContact(xaml));
            SearchCommand = new RelayCommand<string>((text) => SearchContacts(text));

            SortAscCommand = new RelayCommand<object>((xaml) => SortAsc());
            SortDescCommand = new RelayCommand<object>((xaml) => SortDesc());
        }

        private void SearchContacts(string text)
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
                Contacts = new ObservableCollection<ContactEntry>(cache);
            }
            else
            {
                Contacts = new ObservableCollection<ContactEntry>(cache.Where(c => c.CN.ToUpper().StartsWith(searchText.ToUpper())));
            }

            if(isAsc)
                Contacts = new ObservableCollection<ContactEntry>(Contacts.OrderBy(c => c.CN));
            else
                Contacts = new ObservableCollection<ContactEntry>(Contacts.OrderByDescending(c => c.CN));

            OnPropertyChanged(nameof(Contacts));
        }

        private async void AddContact(object xaml)
        {
            IAddContactDialogService dialog = App.Current.Services.GetService<IAddContactDialogService>();
            var result = await dialog.ShowDialog(xaml);
            if (result == true)
            {
                busyService.Busy();

                try
                {
                    await Add(dialog.Contact);
                    notification.ShowSuccessMessage("Contact added");
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

        private async void ModifyContact(object xaml)
        {
            IEditContactDialogService dialog = App.Current.Services.GetService<IEditContactDialogService>();
            dialog.Contact = Contact;
            var result = await dialog.ShowDialog(xaml);
            if (result == true)
            {
                busyService.Busy();

                try
                {
                    Contact = await Modify(dialog.Contact);
                    notification.ShowSuccessMessage("Contact modified");
                    OnPropertyChanged(nameof(Contact));
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

        private async void DeleteContact(object xaml)
        {
            IQuestionDialogService dialog = App.Current.Services.GetService<IQuestionDialogService>();
            var result = await dialog.ShowDialog(xaml, "Delete", "Are you sure you want to delete this contact?");
            if (result == true)
            {
                busyService.Busy();

                try
                {
                    await Delete(Contact);
                    notification.ShowSuccessMessage("Contact deleted");
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
                    using (var contactsRepository = new ContactsRepository(ldap))
                    {
                        cache = await contactsRepository.ListAsync();
                        if (cache == null)
                            cache = new List<ContactEntry>();
                        Contacts = new ObservableCollection<ContactEntry>(cache);
                    }
                }
            });
            OnPropertyChanged(nameof(Contacts));

            busyService.Idle();
        }

        public async Task Add(ContactEntry contact)
        {
            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    using (var contactsRepository = new ContactsRepository(ldap))
                    {
                        if (string.IsNullOrEmpty(contact.CN))
                            contact.CN = contact.DisplayName;
                        await contactsRepository.AddAsync(contact);
                    }
                }
            });
        }

        public async Task<ContactEntry> Modify(ContactEntry contact)
        {
            ContactEntry entry = null;

            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    using (var contactsRepository = new ContactsRepository(ldap))
                    {
                        entry = await contactsRepository.ModifyAsync(contact);
                    }
                }
            });

            return entry;
        }

        public async Task Delete(ContactEntry contact)
        {
            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    using (var contactsRepository = new ContactsRepository(ldap))
                    {
                        await contactsRepository.DeleteAsync(contact);
                    }
                }
            });
        }
    }
}