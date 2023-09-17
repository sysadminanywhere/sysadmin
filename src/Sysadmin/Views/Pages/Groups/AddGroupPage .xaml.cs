using SysAdmin.ActiveDirectory.Models;
using System.DirectoryServices.AccountManagement;
using System.Windows.Controls;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Controls;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class AddGroupPage : INavigableView<ViewModels.AddGroupViewModel>
    {
        public ViewModels.AddGroupViewModel ViewModel
        {
            get;
        }

        public AddGroupPage(ViewModels.AddGroupViewModel viewModel)
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

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewModel.IsSecurity = (bool)radSecurity.IsChecked;

            if (cmbScope.SelectedIndex == 0)
                ViewModel.GroupScope = GroupScopes.Global;

            if (cmbScope.SelectedIndex == 1)
                ViewModel.GroupScope = GroupScopes.Local;

            if (cmbScope.SelectedIndex == 2)
                ViewModel.GroupScope = GroupScopes.Universal;
        }

    }
}
