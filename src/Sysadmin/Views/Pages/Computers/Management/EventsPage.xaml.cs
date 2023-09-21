using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common.Interfaces;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class EventsPage : INavigableView<ViewModels.EventsViewModel>
    {
        public ViewModels.EventsViewModel ViewModel
        {
            get;
        }

        public EventsPage(ViewModels.EventsViewModel viewModel)
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
