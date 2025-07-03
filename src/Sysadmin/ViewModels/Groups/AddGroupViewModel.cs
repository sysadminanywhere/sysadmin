using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SysAdmin.ActiveDirectory.Models;
using System.Threading.Tasks;
using System;
using SysAdmin.ActiveDirectory.Services.Ldap;
using LdapForNet;
using SysAdmin.ActiveDirectory.Repositories;
using Wpf.Ui;

namespace Sysadmin.ViewModels
{
    public partial class AddGroupViewModel : ViewModel
    {
        private bool _isInitialized = false;

        private INavigationService _navigationService;

        [ObservableProperty]
        private GroupEntry _group = new GroupEntry();

        [ObservableProperty]
        private GroupScopes _groupScope = GroupScopes.Global;

        [ObservableProperty]
        private bool _isSecurity = true;

        [ObservableProperty]
        private string _errorMessage;

        public AddGroupViewModel(INavigationService navigationService)
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
            _navigationService.Navigate(typeof(Views.Pages.GroupsPage));
        }

        [RelayCommand]
        private async Task OnAdd()
        {
            try
            {
                await Add(Group, GroupScope, IsSecurity);
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

        public async Task Add(GroupEntry group, GroupScopes groupScope, bool isSecurity)
        {
            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    using (var groupsRepository = new GroupsRepository(ldap))
                    {
                        await groupsRepository.AddAsync(group, groupScope, isSecurity);
                    }
                }
            });
        }

    }
}
