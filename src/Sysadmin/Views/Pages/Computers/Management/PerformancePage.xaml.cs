using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common.Interfaces;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class PerformancePage : INavigableView<ViewModels.PerformanceViewModel>
    {
        public ViewModels.PerformanceViewModel ViewModel
        {
            get;
        }

        public PerformancePage(ViewModels.PerformanceViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }

    }
}
