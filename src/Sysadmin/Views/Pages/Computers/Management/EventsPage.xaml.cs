using System.Windows;
using System.Windows.Controls;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class EventsPage : Wpf.Ui.Controls.INavigableView<ViewModels.EventsViewModel>
    {
        public ViewModels.EventsViewModel ViewModel
        {
            get;
        }

        public EventsPage(ViewModels.EventsViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }

        private void MenuFilter_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menu = (MenuItem)sender;
            foreach (MenuItem item in mnuFilter.Items)
            {
                if (item != menu)
                    item.IsChecked = false;
            }
        }

    }
}
