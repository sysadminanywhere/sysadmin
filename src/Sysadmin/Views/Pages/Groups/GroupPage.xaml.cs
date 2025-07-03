using System.Windows;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class GroupPage : Wpf.Ui.Controls.INavigableView<ViewModels.GroupViewModel>
    {
        public ViewModels.GroupViewModel ViewModel
        {
            get;
        }

        public GroupPage(ViewModels.GroupViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();

            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Group")
            {
                memberOf.MemberOf = ViewModel.Group.MemberOf;
                memberOf.PrimaryGroupId = ViewModel.Group.PrimaryGroupId;

                members.Members = ViewModel.Group.Members;
            }
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to delete this group?", "Delete", MessageBoxButton.YesNo);
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
            //snackbar.Message = ErrorMessage;
            //snackbar.Show();
        }

    }
}
