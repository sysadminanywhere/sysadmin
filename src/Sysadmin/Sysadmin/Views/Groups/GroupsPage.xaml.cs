using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SysAdmin.ActiveDirectory.Models;
using SysAdmin.ViewModels;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SysAdmin.Views.Groups
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GroupsPage : Page
    {

        public GroupsViewModel ViewModel { get; } = new GroupsViewModel();

        public GroupsPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await ViewModel.ListAsync();
        }

        private void groups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is GroupEntry)
            {
                Frame.Navigate(typeof(GroupDetailsPage), e.AddedItems[0]);
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
                ViewModel.FilterCommand.Execute(GroupsViewModel.Filters.All);

            if (mnuFilterBuiltInGroup.IsChecked)
                ViewModel.FilterCommand.Execute(GroupsViewModel.Filters.BuiltIn);

            if (mnuFilterDomainLocalDistribution.IsChecked)
                ViewModel.FilterCommand.Execute(GroupsViewModel.Filters.DomainLocalDistribution);

            if (mnuFilterDomainLocalSecurity.IsChecked)
                ViewModel.FilterCommand.Execute(GroupsViewModel.Filters.DomainLocalSecurity);

            if (mnuFilterGlobalDistribution.IsChecked)
                ViewModel.FilterCommand.Execute(GroupsViewModel.Filters.GlobalDistribution);

            if (mnuFilterGlobalSecurity.IsChecked)
                ViewModel.FilterCommand.Execute(GroupsViewModel.Filters.GlobalSecurity);

            if (mnuFilterUniversalDistribution.IsChecked)
                ViewModel.FilterCommand.Execute(GroupsViewModel.Filters.UniversalDistribution);

            if (mnuFilterUniversalSecurity.IsChecked)
                ViewModel.FilterCommand.Execute(GroupsViewModel.Filters.UniversalSecurity);

        }

    }
}
