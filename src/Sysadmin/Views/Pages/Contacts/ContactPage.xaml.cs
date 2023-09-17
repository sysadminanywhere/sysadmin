using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common.Interfaces;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class ContactPage : INavigableView<ViewModels.ContactViewModel>
    {
        public ViewModels.ContactViewModel ViewModel
        {
            get;
        }

        public ContactPage(ViewModels.ContactViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to delete this contact?", "Delete", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
                ViewModel.DeleteCommand.Execute(ViewModel);
        }

    }
}
