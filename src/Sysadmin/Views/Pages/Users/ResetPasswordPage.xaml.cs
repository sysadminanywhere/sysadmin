using Wpf.Ui.Controls;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class ResetPasswordPage : INavigableView<ViewModels.ResetPasswordViewModel>
    {

        public ViewModels.ResetPasswordViewModel ViewModel
        {
            get;
        }

        public ResetPasswordPage(ViewModels.ResetPasswordViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }

        private void txtPassword_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ViewModel != null)
            {
                ViewModel.Password = ((System.Windows.Controls.PasswordBox)sender).SecurePassword;
            }

        }
    }

}