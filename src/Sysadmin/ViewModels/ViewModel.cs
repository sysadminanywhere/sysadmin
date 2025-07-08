using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;
using Wpf.Ui.Controls;

namespace Sysadmin.ViewModels
{
    public abstract class ViewModel : ObservableObject, INavigationAware
    {
        /// <inheritdoc />
        public virtual Task OnNavigatedToAsync()
        {
            OnNavigatedTo();

            return Task.CompletedTask;
        }

        /// <summary>
        /// Handles the event that is fired after the component is navigated to.
        /// </summary>
        // ReSharper disable once MemberCanBeProtected.Global
        public virtual void OnNavigatedTo() { }

        /// <inheritdoc />
        public virtual Task OnNavigatedFromAsync()
        {
            OnNavigatedFrom();

            return Task.CompletedTask;
        }

        /// <summary>
        /// Handles the event that is fired before the component is navigated from.
        /// </summary>
        // ReSharper disable once MemberCanBeProtected.Global
        public virtual void OnNavigatedFrom() { }
    }

}
