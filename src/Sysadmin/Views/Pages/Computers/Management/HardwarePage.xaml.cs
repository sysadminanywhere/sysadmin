using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common.Interfaces;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class HardwarePage : INavigableView<ViewModels.HardwareViewModel>
    {
        public ViewModels.HardwareViewModel ViewModel
        {
            get;
        }

        public HardwarePage(ViewModels.HardwareViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }

    }
}
