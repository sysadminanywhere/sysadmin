using Wpf.Ui.Common.Interfaces;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class ReportsPage : INavigableView<ViewModels.ReportsViewModel>
    {
        public ViewModels.ReportsViewModel ViewModel
        {
            get;
        }

        public ReportsPage(ViewModels.ReportsViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }
    }
}
