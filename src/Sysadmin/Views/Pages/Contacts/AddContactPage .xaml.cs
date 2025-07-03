
namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class AddContactPage : Wpf.Ui.Controls.INavigableView<ViewModels.AddContactViewModel>
    {
        public ViewModels.AddContactViewModel ViewModel
        {
            get;
        }

        public AddContactPage(ViewModels.AddContactViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }

    }
}
