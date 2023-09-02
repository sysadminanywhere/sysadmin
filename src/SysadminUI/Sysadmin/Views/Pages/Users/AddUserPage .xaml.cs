using System.Windows.Controls;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Controls;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class AddUserPage : INavigableView<ViewModels.UserViewModel>
    {
        public ViewModels.UserViewModel ViewModel
        {
            get;
        }

        public AddUserPage(ViewModels.UserViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();

            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            directoryControl.DistinguishedName = App.CONTAINERS.GetUsersContainer();
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ErrorMessage")
            {
                snackbar.Message = ViewModel.ErrorMessage;
                snackbar.Show();
            }
        }

        private void txtDisplayName_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {

        }

        private void chkUserMustChangePassword_Checked(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void chkPasswordNeverExpires_Checked(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
