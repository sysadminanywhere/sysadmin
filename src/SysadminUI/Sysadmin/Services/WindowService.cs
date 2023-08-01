using Sysadmin.Views.Pages.Computers;
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
    }
}
