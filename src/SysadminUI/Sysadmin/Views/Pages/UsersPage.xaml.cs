using Wpf.Ui.Common.Interfaces;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class UsersPage : INavigableView<ViewModels.UsersViewModel>
    {
        public ViewModels.UsersViewModel ViewModel
        {
            get;
        }

        public UsersPage(ViewModels.UsersViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }
    }
}
