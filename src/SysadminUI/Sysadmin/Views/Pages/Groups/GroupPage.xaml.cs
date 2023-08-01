using System.Windows.Controls;
using Wpf.Ui.Common.Interfaces;

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

    }
}
