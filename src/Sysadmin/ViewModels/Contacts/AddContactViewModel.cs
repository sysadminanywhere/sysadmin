using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SysAdmin.ActiveDirectory.Models;
using System.Threading.Tasks;
using System;
using Wpf.Ui.Mvvm.Contracts;
using LdapForNet;
using SysAdmin.ActiveDirectory.Services.Ldap;
using SysAdmin.ActiveDirectory.Repositories;

namespace Sysadmin.ViewModels
{
    public partial class AddContactViewModel : ViewModel
    {
        private bool _isInitialized = false;

        private INavigationService _navigationService;

        [ObservableProperty]
        private string _distinguishedName;

        [ObservableProperty]
        private ContactEntry _contact = new ContactEntry();

        [ObservableProperty]
        private string _errorMessage;

        public AddContactViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            _isInitialized = true;
        }

        [RelayCommand]
        private void OnClose()
        {
            _navigationService.Navigate(typeof(Views.Pages.ContactsPage));
        }

        [RelayCommand]
        private async Task OnAdd()
        {
            try
            {
                if (string.IsNullOrEmpty(Contact.CN))
                    Contact.CN = Contact.DisplayName;

                if (string.IsNullOrEmpty(Contact.Name))
                    Contact.Name = Contact.DisplayName;

                await Add(Contact);
                _navigationService.Navigate(typeof(Views.Pages.ContactsPage));
            }
            catch (LdapException le)
            {
                ErrorMessage = SysAdmin.ActiveDirectory.LdapResult.GetErrorMessageFromResult(le.ResultCode);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

        }

        public async Task Add(ContactEntry contact)
        {
            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    using (var contactsRepository = new ContactsRepository(ldap))
                    {
                        await contactsRepository.AddAsync(contact);
                    }
                }
            });
        }

    }
}
