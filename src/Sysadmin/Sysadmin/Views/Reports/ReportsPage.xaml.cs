using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SysAdmin.Services.Reports;
using SysAdmin.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SysAdmin.Views.Reports
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ReportsPage : Page
    {

        public ReportsViewModel ViewModel { get; } = new ReportsViewModel();

        public ReportsPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ViewModel.ListAsync();
        }

        private void reports_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is IReport)
            {
                Frame.Navigate(typeof(ReportPreviewPage), e.ClickedItem);
            }
        }

    }
}
