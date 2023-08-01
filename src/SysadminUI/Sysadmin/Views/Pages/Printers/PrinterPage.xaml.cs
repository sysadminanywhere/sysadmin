using System.Windows.Controls;
using Wpf.Ui.Common.Interfaces;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class PrinterPage : INavigableView<ViewModels.PrinterViewModel>
    {
        public ViewModels.PrinterViewModel ViewModel
        {
            get;
        }

        public PrinterPage(ViewModels.PrinterViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }

    }
}
