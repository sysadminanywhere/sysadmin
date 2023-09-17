using System.Windows.Controls;
using Wpf.Ui.Common.Interfaces;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class AddComputerPage : INavigableView<ViewModels.AddComputerViewModel>
    {
        public ViewModels.AddComputerViewModel ViewModel
        {
            get;
        }

        public AddComputerPage(ViewModels.AddComputerViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();

            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            directoryControl.DistinguishedName = App.CONTAINERS.GetComputersContainer();
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
