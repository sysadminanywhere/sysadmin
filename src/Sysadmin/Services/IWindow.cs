using System.Windows;

namespace Sysadmin.Services
{
    public interface IWindow
    {
        event RoutedEventHandler Loaded;

        void Show();
    }
}
