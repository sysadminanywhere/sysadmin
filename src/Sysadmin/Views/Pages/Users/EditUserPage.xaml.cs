using SysAdmin.Services;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using Wpf.Ui.Common.Interfaces;
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

            InitializeComponent();

            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ErrorMessage")
            {
                snackbar.Message = ViewModel.ErrorMessage;
                snackbar.Show();
            }
        }

    }
}
