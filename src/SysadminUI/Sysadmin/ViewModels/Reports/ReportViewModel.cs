using CommunityToolkit.Mvvm.Input;
using Sysadmin.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Sysadmin.Services.Reports;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace Sysadmin.ViewModels
{
    public partial class ReportViewModel : ObservableObject, INavigationAware
    {

        private bool _isInitialized = false;

        private INavigationService _navigationService;

        [ObservableProperty]
        private IReport _report;

        public ReportViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        public void OnNavigatedFrom()
        {
        }

        private void InitializeViewModel()
        {
            _isInitialized = true;
        }


        [RelayCommand]
        private void OnClose()
        {
            _navigationService.Navigate(typeof(Views.Pages.ReportsPage));
        }

    }
}
