using System.Windows.Controls;
using Wpf.Ui.Common.Interfaces;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class AddGroupPage : INavigableView<ViewModels.GroupViewModel>
    {
        public ViewModels.GroupViewModel ViewModel
        {
            get;
        }

        public AddGroupPage(ViewModels.GroupViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }

    }
}
