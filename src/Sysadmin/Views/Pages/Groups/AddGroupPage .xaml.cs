using SysAdmin.ActiveDirectory.Models;
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
            DataContext = this;

            InitializeComponent();
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
