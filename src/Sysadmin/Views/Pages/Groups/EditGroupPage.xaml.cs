using Wpf.Ui.Controls;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class EditGroupPage : INavigableView<ViewModels.EditGroupViewModel>
    {
        public ViewModels.EditGroupViewModel ViewModel
        {
            get;
        }

        public EditGroupPage(ViewModels.EditGroupViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }

    }
}
