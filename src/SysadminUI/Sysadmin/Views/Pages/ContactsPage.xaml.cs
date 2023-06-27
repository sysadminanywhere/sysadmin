using Wpf.Ui.Common.Interfaces;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class ContactsPage : INavigableView<ViewModels.ContactsViewModel>
    {
        public ViewModels.ContactsViewModel ViewModel
        {
            get;
        }

        public ContactsPage(ViewModels.ContactsViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }
    }
}
