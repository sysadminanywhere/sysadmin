using CommunityToolkit.Mvvm.ComponentModel;
using Sysadmin.Models;
using SysAdmin.ActiveDirectory.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Wpf.Ui.Common.Interfaces;

namespace Sysadmin.ViewModels
{
    public partial class ComputersViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        [ObservableProperty]
        private IEnumerable<ComputerEntry> _computers;

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
            Computers = new ObservableCollection<ComputerEntry>();

            _isInitialized = true;
        }
    }
}
