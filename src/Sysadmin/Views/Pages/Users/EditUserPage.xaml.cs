using SysAdmin.Services;
using Wpf.Ui.Controls;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class EditUserPage : INavigableView<ViewModels.EditUserViewModel>
    {

        public ViewModels.EditUserViewModel ViewModel
        {
            get;
        }

        public EditUserPage(ViewModels.EditUserViewModel viewModel, ISettingsService settingsService)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }

    }
}
