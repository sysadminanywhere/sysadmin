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
    public partial class EditContactPage : INavigableView<ViewModels.ContactViewModel>
    {

        private ISettingsService _settingsService;

        public ViewModels.ContactViewModel ViewModel
        {
            get;
        }

        public EditContactPage(ViewModels.ContactViewModel viewModel, ISettingsService settingsService)
        {
            ViewModel = viewModel;
            _settingsService = settingsService;

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
