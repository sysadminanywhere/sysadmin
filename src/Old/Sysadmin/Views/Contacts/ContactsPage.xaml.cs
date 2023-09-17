using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SysAdmin.ActiveDirectory.Models;
using SysAdmin.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SysAdmin.Views.Contacts
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ContactsPage : Page
    {

        public ContactsViewModel ViewModel { get; } = new ContactsViewModel();

        public ContactsPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await ViewModel.ListAsync();
        }

        private void contacts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is ContactEntry)
            {
                Frame.Navigate(typeof(ContactDetailsPage), e.AddedItems[0]);
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
    }
}
