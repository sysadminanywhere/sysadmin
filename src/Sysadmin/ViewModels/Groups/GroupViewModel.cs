using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Sysadmin.Services;
using SysAdmin.ActiveDirectory.Models;
using System.Threading.Tasks;
using System;
using Wpf.Ui.Mvvm.Contracts;
using SysAdmin.ActiveDirectory.Services.Ldap;
using LdapForNet;
using SysAdmin.ActiveDirectory.Repositories;

namespace Sysadmin.ViewModels
{
    public partial class GroupViewModel : ViewModel
    {
        private bool _isInitialized = false;

        private INavigationService _navigationService;
        private IExchangeService _exchangeService;

        [ObservableProperty]
        private GroupEntry _group = new GroupEntry();

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        public GroupViewModel(INavigationService navigationService, IExchangeService exchangeService)
        {
            _navigationService = navigationService;
            _exchangeService = exchangeService;
        }

        public override void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();

            if (_exchangeService.GetParameter() is GroupEntry entry)
                Group = entry;
        }

        private void InitializeViewModel()
        {
            _isInitialized = true;
        }

        [RelayCommand]
        private void OnClose()
        {
            _navigationService.Navigate(typeof(Views.Pages.GroupsPage));
        }

        [RelayCommand]
        private void OnEdit()
        {
            _navigationService.Navigate(typeof(Views.Pages.EditGroupPage));
        }

        [RelayCommand]
        private async Task OnDelete()
        {
            try
            {
                await Delete(Group);
                _navigationService.Navigate(typeof(Views.Pages.GroupsPage));
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

        public async Task Get()
        {
            GroupEntry entry = Group;

            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    using (var groupsRepository = new GroupsRepository(ldap))
                    {
                        entry = await groupsRepository.GetByCNAsync(Group.CN);
                    }
                }
            });

            Group = entry;
        }

    }
}
