using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common.Interfaces;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class SoftwarePage : INavigableView<ViewModels.SoftwareViewModel>
    {
        public ViewModels.SoftwareViewModel ViewModel
        {
            get;
        }

        public SoftwarePage(ViewModels.SoftwareViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }

    }
}
