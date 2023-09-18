using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SysAdmin.ActiveDirectory.Models;
using SysAdmin.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SysAdmin.Views.Groups
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GroupDetailsPage : Page
    {

        public GroupsViewModel ViewModel { get; } = new GroupsViewModel();

        public GroupDetailsPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is GroupEntry)
                ViewModel.Group = (GroupEntry)e.Parameter;
        }

        private async void MembersControl_Changed()
        {
            await ViewModel.Get(ViewModel.Group.CN);
        }

        private async void MemberOfControl_Changed()
        {
            await ViewModel.Get(ViewModel.Group.CN);
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ADPropertiesPage), ViewModel.Group.DistinguishedName);
        }
    }
}