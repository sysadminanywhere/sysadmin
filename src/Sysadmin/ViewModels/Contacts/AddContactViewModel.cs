using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LdapForNet;
using SysAdmin.ActiveDirectory;
using SysAdmin.ActiveDirectory.Models;
using SysAdmin.ActiveDirectory.Repositories;
using SysAdmin.ActiveDirectory.Services.Ldap;
using System;
using System.Threading.Tasks;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace Sysadmin.ViewModels
{
    public partial class AddContactViewModel : ViewModel
    {
        private bool isInitialized = false;

        private INavigationService navigationService;
        private ISnackbarService snackbarService;

        [ObservableProperty]
        private string _distinguishedName;

        [ObservableProperty]
        private ContactEntry _contact = new ContactEntry();

        public AddContactViewModel(INavigationService navigationService, ISnackbarService snackbarService)
        {
            this.navigationService = navigationService;
            this.snackbarService = snackbarService;
        }

        public override void OnNavigatedTo()
        {
            if (!isInitialized)
                InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            isInitialized = true;
        }

        [RelayCommand]
        private void OnClose()
        {
            navigationService.GoBack();
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
                navigationService.Navigate(typeof(Views.Pages.ContactsPage));
            }
            catch (LdapException le)
            {
                snackbarService.Show("Error",
                    LdapResult.GetErrorMessageFromResult(le.ResultCode),
                    ControlAppearance.Secondary,
                    new SymbolIcon(SymbolRegular.ErrorCircle12),
                    TimeSpan.FromSeconds(5)
                );
            }
            catch (Exception ex)
            {
                snackbarService.Show("Error",
                    ex.Message,
                    ControlAppearance.Secondary,
                    new SymbolIcon(SymbolRegular.ErrorCircle12),
                    TimeSpan.FromSeconds(5)
                );
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
