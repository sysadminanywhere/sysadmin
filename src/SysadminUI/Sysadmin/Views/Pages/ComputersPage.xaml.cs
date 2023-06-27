using Wpf.Ui.Common.Interfaces;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class ComputersPage : INavigableView<ViewModels.ComputersViewModel>
    {
        public ViewModels.ComputersViewModel ViewModel
        {
            get;
        }

        public ComputersPage(ViewModels.ComputersViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }
    }
}
