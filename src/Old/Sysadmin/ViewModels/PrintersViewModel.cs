using LdapForNet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using SysAdmin.ActiveDirectory.Models;
using SysAdmin.ActiveDirectory.Repositories;
using SysAdmin.ActiveDirectory.Services.Ldap;
using SysAdmin.Services;
using SysAdmin.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SysAdmin.ViewModels
{
    public class PrintersViewModel : ObservableObject
    {

        public PrinterEntry Printer { get; set; } = new PrinterEntry();
        public ObservableCollection<PrinterEntry> Printers { get; private set; } = new ObservableCollection<PrinterEntry>();

        public RelayCommand DeleteCommand { get; private set; }
        public RelayCommand<string> SearchCommand { get; private set; }

        INavigationService navigation = App.Current.Services.GetService<INavigationService>();
        INotificationService notification = App.Current.Services.GetService<INotificationService>();
        IBusyService busyService = App.Current.Services.GetService<IBusyService>();

        private List<PrinterEntry> cache;

        private string searchText = string.Empty;
        private bool isAsc = true;

        public RelayCommand SortAscCommand { get; private set; }
        public RelayCommand SortDescCommand { get; private set; }

        public PrintersViewModel()
        {
            DeleteCommand = new RelayCommand(() => DeletePrinter());
            SearchCommand = new RelayCommand<string>((text) => SearchPrinters(text));

            SortAscCommand = new RelayCommand(() => SortAsc());
            SortDescCommand = new RelayCommand(() => SortDesc());
        }

        private void SearchPrinters(string text)
        {
            searchText = text;
            SortingAndFiltering();
        }

        private void SortDesc()
        {
            isAsc = false;
            SortingAndFiltering();
        }

        private void SortAsc()
        {
            isAsc = true;
            SortingAndFiltering();
        }

        private void SortingAndFiltering()
        {
            if (cache != null)
            {
                if (string.IsNullOrEmpty(searchText))
                {
                    Printers = new ObservableCollection<PrinterEntry>(cache);
                }
                else
                {
                    Printers = new ObservableCollection<PrinterEntry>(cache.Where(c => c.CN.ToUpper().StartsWith(searchText.ToUpper())));
                }

                if (isAsc)
                    Printers = new ObservableCollection<PrinterEntry>(Printers.OrderBy(c => c.CN));
                else
                    Printers = new ObservableCollection<PrinterEntry>(Printers.OrderByDescending(c => c.CN));

                OnPropertyChanged(nameof(Printers));
            }
        }

        private async void DeletePrinter()
        {
            IQuestionDialogService dialog = App.Current.Services.GetService<IQuestionDialogService>();
            var result = await dialog.ShowDialog("Delete", "Are you sure you want to delete this printer?");
            if (result == true)
            {
                busyService.Busy();

                try
                {
                    await Delete(Printer);
                    notification.ShowSuccessMessage("Printer deleted");
                    if (navigation.CanGoBack) navigation.GoBack();
                }
                catch (LdapException le)
                {
                    notification.ShowErrorMessage(ActiveDirectory.LdapResult.GetErrorMessageFromResult(le.ResultCode));
                }
                catch (Exception ex)
                {
                    notification.ShowErrorMessage(ex.Message);
                }
            }

            busyService.Idle();
        }

        public async Task ListAsync()
        {
            busyService.Busy();

            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    using (var printersRepository = new PrintersRepository(ldap))
                    {
                        cache = await printersRepository.ListAsync();
                        if (cache == null)
                            cache = new List<PrinterEntry>();
                        Printers = new ObservableCollection<PrinterEntry>(cache);
                    }
                }
            });
            OnPropertyChanged(nameof(Printers));

            busyService.Idle();
        }

        public async Task Delete(PrinterEntry printer)
        {
            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    using (var printersRepository = new PrintersRepository(ldap))
                    {
                        await printersRepository.DeleteAsync(printer);
                    }
                }
            });

        }

    }
}