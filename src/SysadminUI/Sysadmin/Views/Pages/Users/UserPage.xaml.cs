using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common.Interfaces;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class UserPage : INavigableView<ViewModels.UserViewModel>
    {
        public ViewModels.UserViewModel ViewModel
        {
            get;
        }

        public UserPage(ViewModels.UserViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure?", "Delete", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
                ViewModel.DeleteCommand.Execute(ViewModel);
        }

    }
}