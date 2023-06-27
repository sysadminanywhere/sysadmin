using Wpf.Ui.Common.Interfaces;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class PrintersPage : INavigableView<ViewModels.PrintersViewModel>
    {
        public ViewModels.PrintersViewModel ViewModel
        {
            get;
        }

        public PrintersPage(ViewModels.PrintersViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }
    }
}
