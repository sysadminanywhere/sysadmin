using Sysadmin.Views.Pages.Computers;
using Sysadmin.Views.Pages.Contacts;
using Sysadmin.Views.Pages.Groups;
using Sysadmin.Views.Pages.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sysadmin.Services
{
    public class WindowService : IWindowService
    {
        public void AddComputerWindow()
        {
            var win = new AddComputerWindow();
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
