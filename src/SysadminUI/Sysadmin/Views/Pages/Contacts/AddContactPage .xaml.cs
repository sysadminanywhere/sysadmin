using System.Windows.Controls;
using Wpf.Ui.Common.Interfaces;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class AddContactPage : INavigableView<ViewModels.ContactViewModel>
    {
        public ViewModels.ContactViewModel ViewModel
        {
            get;
        }

        public AddContactPage(ViewModels.ContactViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }

    }
}
