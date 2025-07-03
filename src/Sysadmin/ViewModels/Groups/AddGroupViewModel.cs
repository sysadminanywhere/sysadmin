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
    public partial class AddGroupViewModel : ViewModel
    {
        private bool isInitialized = false;

        private INavigationService navigationService;
        private ISnackbarService snackbarService;

        [ObservableProperty]
        private GroupEntry _group = new GroupEntry();

        [ObservableProperty]
        private GroupScopes _groupScope = GroupScopes.Global;

        [ObservableProperty]
        private bool _isSecurity = true;

        public AddGroupViewModel(INavigationService navigationService, ISnackbarService snackbarService)
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
                await Add(Group, GroupScope, IsSecurity);
                navigationService.Navigate(typeof(Views.Pages.GroupsPage));
            }
            catch (LdapException le)
            {
                snackbarService.Show("Error",
                    LdapResult.GetErrorMessageFromResult(le.ResultCode),
                    ControlAppearance.Danger,
                    new SymbolIcon(SymbolRegular.ErrorCircle12),
                    TimeSpan.FromSeconds(5)
                );
            }
            catch (Exception ex)
            {
                snackbarService.Show("Error",
                    ex.Message,
                    ControlAppearance.Danger,
                    new SymbolIcon(SymbolRegular.ErrorCircle12),
                    TimeSpan.FromSeconds(5)
                );
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
