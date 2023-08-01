using System.Windows.Controls;
using Wpf.Ui.Common.Interfaces;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class ComputerPage : INavigableView<ViewModels.ComputerViewModel>
    {
        public ViewModels.ComputerViewModel ViewModel
        {
            get;
        }

        public ComputerPage(ViewModels.ComputerViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }

    }
}
