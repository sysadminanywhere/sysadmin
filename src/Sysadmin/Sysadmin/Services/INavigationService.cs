namespace SysAdmin.Services
{
    public interface INavigationService
    {
        bool CanGoBack { get; }
        void GoBack();
        void Navigate<T>(object args = null);
    }
}
