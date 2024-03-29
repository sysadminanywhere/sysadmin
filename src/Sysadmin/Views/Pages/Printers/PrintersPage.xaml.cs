﻿using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Controls;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class PrintersPage : INavigableView<ViewModels.PrintersViewModel>
    {
        public ViewModels.PrintersViewModel ViewModel
        {
            get;
        }

        public PrintersPage(ViewModels.PrintersViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }

        private void MenuSort_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MenuItem menu = (MenuItem)sender;
            foreach (MenuItem item in mnuSort.Items)
            {
                if (item != menu)
                    item.IsChecked = false;
            }
        }

        private void AutoSuggestBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (sender is AutoSuggestBox)
                ViewModel.SearchCommand.Execute(((AutoSuggestBox)sender).Text);
        }
    }
}
