using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysAdmin.Services
{
    public interface INotificationService
    {
        void ShowErrorMessage(string message);
        void ShowWarningMessage(string message);
        void ShowInformationalMessage(string message);
        void ShowSuccessMessage(string message);
    }
}
