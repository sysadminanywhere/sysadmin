using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SysAdmin.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SysAdmin.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ADPropertiesPage : Page
    {

        public ADPropertiesViewModel ViewModel { get; } = new ADPropertiesViewModel();

        public ADPropertiesPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is string)
            {
                ViewModel.DistinguishedName = e.Parameter.ToString();
                await ViewModel.LoadAsync();
            }
        }

    }
}