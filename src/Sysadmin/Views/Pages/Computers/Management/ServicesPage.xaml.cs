using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common.Interfaces;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class ServicesPage : INavigableView<ViewModels.ServicesViewModel>
    {
        public ViewModels.ServicesViewModel ViewModel
        {
            get;
        }

        public ServicesPage(ViewModels.ServicesViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }

    }
}
