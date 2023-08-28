using Sysadmin.Views.Pages.Computers;
using Sysadmin.Views.Pages.Contacts;
using Sysadmin.Views.Pages.Groups;
using Sysadmin.Views.Pages.Users;
using Wpf.Ui.Mvvm.Contracts;

namespace Sysadmin.Services
{
    public class WindowService : IWindowService
    {

        private INavigationService _navigationService;
        private IExchangeService _exchangeService;

        public WindowService(INavigationService navigationService, IExchangeService exchangeService)
        {
            _navigationService = navigationService;
            _exchangeService = exchangeService;
        }

        public void AddComputerWindow()
        {
            var win = new AddComputerWindow(new ViewModels.ComputerViewModel(_navigationService, _exchangeService));
            win.ShowDialog();
        }

        public void AddContactWindow()
        {
            var win = new AddContactWindow();
            win.ShowDialog();
        }

        public void AddGroupWindow()
        {
            var win = new AddGroupWindow();
            win.ShowDialog();
        }

        public void AddUserWindow()
        {
            var win = new AddUserWindow();
            win.ShowDialog();
        }
    }
}
