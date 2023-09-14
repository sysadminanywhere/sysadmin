using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common.Interfaces;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class ReportPage : INavigableView<ViewModels.ReportViewModel>
    {
        public ViewModels.ReportViewModel ViewModel
        {
            get;
        }

        public ReportPage(ViewModels.ReportViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }

    }
}
