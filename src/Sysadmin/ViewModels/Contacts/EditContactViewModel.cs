using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Sysadmin.Services;
using SysAdmin.ActiveDirectory.Models;
using System.Threading.Tasks;
using System;
using Wpf.Ui.Mvvm.Contracts;
using LdapForNet;
using SysAdmin.ActiveDirectory.Services.Ldap;
using SysAdmin.ActiveDirectory.Repositories;

namespace Sysadmin.ViewModels
{
    public partial class EditContactViewModel : ViewModel
    {
        private bool _isInitialized = false;

        private INavigationService _navigationService;
        private IExchangeService _exchangeService;

        [ObservableProperty]
        private string _distinguishedName = string.Empty;

        [ObservableProperty]
        private ContactEntry _contact = new ContactEntry();

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        public EditContactViewModel(INavigationService navigationService, IExchangeService exchangeService)
        {
            _navigationService = navigationService;
            _exchangeService = exchangeService;
        }

        public override void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();

            if (_exchangeService.GetParameter() is ContactEntry entry)
                Contact = entry;
        }

        private void InitializeViewModel()
        {
            _isInitialized = true;
        }

        [RelayCommand]
        private void OnClose()
        {
            _navigationService.Navigate(typeof(Views.Pages.ContactPage));
        }

        [RelayCommand]
        private async Task OnEdit()
        {
            try
            {
                await Edit(Contact);
                _navigationService.Navigate(typeof(Views.Pages.ContactPage));
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

        public async Task Edit(ContactEntry contact)
        {
            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    using (var contactsRepository = new ContactsRepository(ldap))
                    {
                        await contactsRepository.ModifyAsync(contact);
                    }
                }
            });
        }

    }
}
