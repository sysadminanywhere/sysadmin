using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common.Interfaces;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class ProcessesPage : INavigableView<ViewModels.ProcessesViewModel>
    {
        public ViewModels.ProcessesViewModel ViewModel
        {
            get;
        }

        public ProcessesPage(ViewModels.ProcessesViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }

    }
}
