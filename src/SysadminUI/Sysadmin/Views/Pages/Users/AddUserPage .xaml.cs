using System.Windows.Controls;
using Wpf.Ui.Common.Interfaces;

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
        }

    }
}
