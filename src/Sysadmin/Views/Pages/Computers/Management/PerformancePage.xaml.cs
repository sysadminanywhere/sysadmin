using System;
using System.Windows;
using System.Windows.Threading;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class PerformancePage : Wpf.Ui.Controls.INavigableView<ViewModels.PerformanceViewModel>
    {

        private DispatcherTimer timer;

        public ViewModels.PerformanceViewModel ViewModel
        {
            get;
        }

        public PerformancePage(ViewModels.PerformanceViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();

            this.Loaded += PerformancePage_Loaded;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsClosed" && ViewModel.IsClosed)
            {
                timer?.Stop();
            }
        }

        private void PerformancePage_Loaded(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object? sender, System.EventArgs e)
        {
            if (ViewModel.totalPhysicalMemory > 0)
            {
                ViewModel.Update();
            }
        }

    }
}