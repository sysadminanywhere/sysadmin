using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Controls;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class GroupPage : INavigableView<ViewModels.GroupViewModel>
    {
        public ViewModels.GroupViewModel ViewModel
        {
            get;
        }

        public GroupPage(ViewModels.GroupViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var result = System.Windows.MessageBox.Show("Are you sure you want to delete this group?", "Delete", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
                ViewModel.DeleteCommand.Execute(ViewModel);
        }

        private async void MembersControl_Changed() //NOSONAR
        {
            await ViewModel.Get();
        }

        private async void MemberOfControl_Changed() //NOSONAR
        {
            await ViewModel.Get();
        }

        private void MemberOfControl_Error(string ErrorMessage) //NOSONAR
        {
            snackbar.Message = ErrorMessage;
            snackbar.Show();
        }

    }
}
