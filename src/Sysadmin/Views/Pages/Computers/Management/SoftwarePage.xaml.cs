
namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class SoftwarePage : Wpf.Ui.Controls.INavigableView<ViewModels.SoftwareViewModel>
    {
        public ViewModels.SoftwareViewModel ViewModel
        {
            get;
        }

        public SoftwarePage(ViewModels.SoftwareViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }

    }
}
