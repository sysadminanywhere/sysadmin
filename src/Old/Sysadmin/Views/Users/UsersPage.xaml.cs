using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SysAdmin.ActiveDirectory.Models;
using SysAdmin.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SysAdmin.Views.Users
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UsersPage : Page
    {

        public UsersViewModel ViewModel { get; } = new UsersViewModel();

        public UsersPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await ViewModel.ListAsync();
        }

        private void users_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is UserEntry)
            {
                Frame.Navigate(typeof(UserDetailsPage), e.AddedItems[0]);
            }
        }

        private void mnuSort_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            ToggleMenuFlyoutItem menu = (ToggleMenuFlyoutItem)sender;
            foreach (ToggleMenuFlyoutItem item in mnuSort.Items)
            {
                if (item != menu)
                    item.IsChecked = false;
            }
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            ViewModel.SearchCommand.Execute(sender.Text);
        }

        private void mnuFilter_Click(object sender, RoutedEventArgs e)
        {
            ToggleMenuFlyoutItem menu = (ToggleMenuFlyoutItem)sender;
            foreach (ToggleMenuFlyoutItem item in mnuFilter.Items)
            {
                if (item != menu)
                    item.IsChecked = false;
            }

            if (mnuFilterAll.IsChecked)
                ViewModel.FilterCommand.Execute(UsersViewModel.Filters.All);

            if (mnuFilterEnabled.IsChecked)
                ViewModel.FilterCommand.Execute(UsersViewModel.Filters.AccountEnabled);

            if (mnuFilterDisabled.IsChecked)
                ViewModel.FilterCommand.Execute(UsersViewModel.Filters.AccountDisabled);

            if (mnuFilterLocked.IsChecked)
                ViewModel.FilterCommand.Execute(UsersViewModel.Filters.Locked);

            if (mnuFilterExpired.IsChecked)
                ViewModel.FilterCommand.Execute(UsersViewModel.Filters.PasswordExpired);

            if (mnuFilterNeverExpires.IsChecked)
                ViewModel.FilterCommand.Execute(UsersViewModel.Filters.NeverExpires);

        }

    }
}
