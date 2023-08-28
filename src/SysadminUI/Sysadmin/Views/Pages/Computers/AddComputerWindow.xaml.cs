using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Controls;

namespace Sysadmin.Views.Pages.Computers
{
    /// <summary>
    /// Interaction logic for AddComputerWindow.xaml
    /// </summary>
    public partial class AddComputerWindow : UiWindow
    {

        public ViewModels.ComputerViewModel ViewModel
        {
            get;
        }

        public AddComputerWindow(ViewModels.ComputerViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewModel.AddAsync();

            this.Close();
        }
    }

}