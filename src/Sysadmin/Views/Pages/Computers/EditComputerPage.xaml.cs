
using Wpf.Ui.Controls;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class EditComputerPage : INavigableView<ViewModels.EditComputerViewModel>
    {
        public ViewModels.EditComputerViewModel ViewModel
        {
            get;
        }

        public EditComputerPage(ViewModels.EditComputerViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }

    }
}
