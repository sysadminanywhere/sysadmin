using Microsoft.UI.Xaml.Controls;
using SysAdmin.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysAdmin.Services
{
    public class NotificationService : INotificationService
    {

        private AutoClosingInfoBar infoBar;

        public void SetInfoBar(AutoClosingInfoBar infoBar)
        {
            this.infoBar = infoBar;
        }

        public void ShowErrorMessage(string message)
        {
            infoBar.Show(InfoBarSeverity.Error, "Error", message);
        }

        public void ShowInformationalMessage(string message)
        {
            infoBar.Show(InfoBarSeverity.Informational, "Information", message);
        }

        public void ShowWarningMessage(string message)
        {
            infoBar.Show(InfoBarSeverity.Warning, "Warning", message);
        }

        public void ShowSuccessMessage(string message)
        {
            infoBar.Show(InfoBarSeverity.Success, "Success", message);
        }

    }
}
