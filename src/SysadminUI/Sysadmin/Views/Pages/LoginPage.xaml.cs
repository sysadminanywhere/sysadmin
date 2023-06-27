using Wpf.Ui.Common.Interfaces;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class LoginPage : INavigableView<ViewModels.LoginViewModel>
    {
        public ViewModels.LoginViewModel ViewModel
        {
            get;
        }

        public LoginPage(ViewModels.LoginViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }
    }
}
