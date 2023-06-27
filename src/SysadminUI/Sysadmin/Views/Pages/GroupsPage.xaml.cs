using Wpf.Ui.Common.Interfaces;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class GroupsPage : INavigableView<ViewModels.GroupsViewModel>
    {
        public ViewModels.GroupsViewModel ViewModel
        {
            get;
        }

        public GroupsPage(ViewModels.GroupsViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }
    }
}
