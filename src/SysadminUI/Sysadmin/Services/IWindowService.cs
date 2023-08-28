using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sysadmin.Services
{
    public interface IWindowService
    {
        void AddComputerWindow();
        void AddUserWindow();
        void AddGroupWindow();
        void AddContactWindow();
    }
}