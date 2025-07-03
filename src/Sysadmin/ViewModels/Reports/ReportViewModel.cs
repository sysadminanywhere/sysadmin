using CommunityToolkit.Mvvm.Input;
using Sysadmin.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Sysadmin.Services.Reports;
using Wpf.Ui;

namespace Sysadmin.ViewModels
{
    public partial class ReportViewModel : ViewModel
    {

        private bool isInitialized = false;

        private INavigationService navigationService;
        private IExchangeService exchangeService;


        [ObservableProperty]
        private IReport _report;

        [ObservableProperty]
        private bool _isBusy;

        public ReportViewModel(INavigationService navigationService, IExchangeService exchangeService)
        {
            this.navigationService = navigationService;
            this.exchangeService = exchangeService;
        }

        public override void OnNavigatedTo()
        {
            if (!isInitialized)
                InitializeViewModel();

            if (exchangeService.GetParameter() is IReport entry)
                Report = entry;
        }

        private void InitializeViewModel()
        {
            isInitialized = true;
        }


        [RelayCommand]
        private void OnClose()
        {
            navigationService.Navigate(typeof(Views.Pages.ReportsPage));
        }

    }
}
