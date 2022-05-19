using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace SysAdmin.Services
{
    public class BusyService : IBusyService
    {

        private ProgressRing progressRing;

        public void Busy()
        {
            progressRing.Visibility = Visibility.Visible;
        }

        public void Idle()
        {
            progressRing.Visibility = Visibility.Collapsed;
        }

        public void SetProgressRing(ProgressRing progressRing)
        {
            this.progressRing = progressRing;
            this.progressRing.Visibility = Visibility.Collapsed;
        }

    }
}