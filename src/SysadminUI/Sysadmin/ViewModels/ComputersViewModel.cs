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

        //[ObservableProperty]
        //private IEnumerable<DataColor> _colors;

        public ObservableCollection<ComputerEntry> Computers { get; private set; } = new ObservableCollection<ComputerEntry>();

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
    }
}
