using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysAdmin.Controls
{
    /// <summary>
    /// An InfoBar that closes itself after an interval.
    /// </summary>
    public class AutoClosingInfoBar : Microsoft.UI.Xaml.Controls.InfoBar
    {
        private DispatcherTimer _timer;
        private long _token;

        public AutoClosingInfoBar() : base()
        {
            this.Loaded += AutoClosingInfoBar_Loaded;
            this.Unloaded += AutoClosingInfoBar_Unloaded;
        }

        /// <summary>
        /// Gets or sets the auto-close interval, in milliseconds.
        /// </summary>
        public int AutoCloseInterval { get; set; } = 5000;

        public void Show(InfoBarSeverity severity, string title, string message)
        {
            this.Title = title;
            this.Message = message;
            this.Severity = severity;
            this.IsOpen = true;
        }

        private void AutoClosingInfoBar_Loaded(object sender, RoutedEventArgs e)
        {
            _token = this.RegisterPropertyChangedCallback(IsOpenProperty, IsOpenChanged);
            if (IsOpen)
            {
                Open();
            }
        }

        private void AutoClosingInfoBar_Unloaded(object sender, RoutedEventArgs e)
        {
            this.UnregisterPropertyChangedCallback(IsOpenProperty, _token);
        }

        private void IsOpenChanged(DependencyObject o, DependencyProperty p)
        {
            var that = o as AutoClosingInfoBar;
            if (that == null)
            {
                return;
            }

            if (p != IsOpenProperty)
            {
                return;
            }

            if (that.IsOpen)
            {
                that.Open();
            }
            else
            {
                that.Close();
            }
        }

        private void Open()
        {
            _timer = new DispatcherTimer();
            _timer.Tick += Timer_Tick;
            _timer.Interval = TimeSpan.FromMilliseconds(AutoCloseInterval);
            _timer.Start();
        }

        private void Close()
        {
            if (_timer == null)
            {
                return;
            }

            _timer.Stop();
            _timer.Tick -= Timer_Tick;
        }

        private void Timer_Tick(object sender, object e)
        {
            this.IsOpen = false;
        }
    }
}