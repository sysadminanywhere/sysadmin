using System.Windows;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class ContactPage : Wpf.Ui.Controls.INavigableView<ViewModels.ContactViewModel>
    {
        public ViewModels.ContactViewModel ViewModel
        {
            get;
        }

        public ContactPage(ViewModels.ContactViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

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
