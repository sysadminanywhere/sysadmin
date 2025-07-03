
using Wpf.Ui.Controls;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class AddComputerPage : INavigableView<ViewModels.AddComputerViewModel>
    {
        public ViewModels.AddComputerViewModel ViewModel
        {
            get;
        }

        public AddComputerPage(ViewModels.AddComputerViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();

            directoryControl.DistinguishedName = App.CONTAINERS.GetComputersContainer();
        }

    }
}
