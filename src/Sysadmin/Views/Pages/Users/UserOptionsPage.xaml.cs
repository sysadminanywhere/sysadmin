using Wpf.Ui.Controls;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class UserOptionsPage : INavigableView<ViewModels.UserOptionsViewModel>
    {

        public ViewModels.UserOptionsViewModel ViewModel
        {
            get;
        }

        public UserOptionsPage(ViewModels.UserOptionsViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }

        private void chkUserMustChangePassword_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (chkPasswordNeverExpires != null)
            {
                chkPasswordNeverExpires.IsEnabled = false;
                chkPasswordNeverExpires.IsChecked = false;
            }
        }

        private void chkUserMustChangePassword_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            chkPasswordNeverExpires.IsEnabled = true;
        }

        private void chkPasswordNeverExpires_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            chkUserMustChangePassword.IsEnabled = false;
            chkUserMustChangePassword.IsChecked = false;
        }

        private void chkPasswordNeverExpires_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            chkUserMustChangePassword.IsEnabled = true;
        }

    }

}