using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LdapForNet;
using Sysadmin.Services;
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
    public partial class EditGroupViewModel : ViewModel
    {
        private bool isInitialized = false;

        private INavigationService navigationService;
        private IExchangeService exchangeService;
        private ISnackbarService snackbarService;

        [ObservableProperty]
        private GroupEntry _group = new GroupEntry();

        public EditGroupViewModel(INavigationService navigationService, IExchangeService exchangeService, ISnackbarService snackbarService)
        {
            this.navigationService = navigationService;
            this.exchangeService = exchangeService;
            this.snackbarService = snackbarService;
        }

        public override void OnNavigatedTo()
        {
            if (!isInitialized)
                InitializeViewModel();

            if (exchangeService.GetParameter() is GroupEntry entry)
                Group = entry;
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
        private async Task OnEdit()
        {
            try
            {
                await Edit(Group);
                navigationService.Navigate(typeof(Views.Pages.GroupPage));
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

        public async Task Edit(GroupEntry group)
        {
            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    using (var groupsRepository = new GroupsRepository(ldap))
                    {
                        await groupsRepository.ModifyAsync(group);
                    }
                }
            });
        }

    }
}
