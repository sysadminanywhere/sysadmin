using CommunityToolkit.Mvvm.Input;
using Sysadmin.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Sysadmin.Services.Reports;
using Wpf.Ui;

namespace Sysadmin.ViewModels
{
    public partial class ReportViewModel : ViewModel
    {

        private bool _isInitialized = false;

        private INavigationService _navigationService;
        private IExchangeService _exchangeService;


        [ObservableProperty]
        private IReport _report;

        [ObservableProperty]
        private bool _isBusy;

        public ReportViewModel(INavigationService navigationService, IExchangeService exchangeService)
        {
            _navigationService = navigationService;
            _exchangeService = exchangeService;
        }

        public override void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();

            if (_exchangeService.GetParameter() is IReport entry)
                Report = entry;
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
