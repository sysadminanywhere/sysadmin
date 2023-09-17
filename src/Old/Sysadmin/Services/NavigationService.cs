using Microsoft.UI.Xaml.Controls;

namespace SysAdmin.Services
{
    public class NavigationService : INavigationService
    {
        private Frame frame;

        public void SetFrame(Frame frame)
        {
            this.frame = frame;
        }

        public bool CanGoBack => this.frame.CanGoBack;

        public void GoBack() => this.frame.GoBack();

        public void Navigate<T>(object args)
        {
            this.frame.Navigate(typeof(T), args);
        }
    }
}
