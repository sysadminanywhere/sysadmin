using SysAdmin.Services;
using Wpf.Ui.Controls;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class EditContactPage : INavigableView<ViewModels.EditContactViewModel>
    {

        public ViewModels.EditContactViewModel ViewModel
        {
            get;
        }

        public EditContactPage(ViewModels.EditContactViewModel viewModel, ISettingsService settingsService)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }

    }
}
