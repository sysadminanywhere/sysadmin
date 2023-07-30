using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Common.Interfaces;

namespace Sysadmin.ViewModels
{
    public partial class UsersViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        [ObservableProperty]
        private bool _sortAsc = true;

        [ObservableProperty]
        private bool _sortDesc = false;

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
        private void OnAdd()
        {
            SortAsc = true;
        }

        [RelayCommand]
        private void OnSortAsc()
        {
            SortAsc = true;
            SortDesc = false;
        }

        [RelayCommand]
        private void OnSortDesc()
        {
            SortAsc = false;
            SortDesc = true;
        }

    }
}
