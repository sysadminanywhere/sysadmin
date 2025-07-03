using System;
using System.Windows;
using Wpf.Ui;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class GroupPage : Wpf.Ui.Controls.INavigableView<ViewModels.GroupViewModel>
    {
        private ISnackbarService snackbarService;

        public ViewModels.GroupViewModel ViewModel
        {
            get;
        }

        public GroupPage(ViewModels.GroupViewModel viewModel, ISnackbarService snackbarService)
        {
            ViewModel = viewModel;
            DataContext = this;

            this.snackbarService = snackbarService;

            InitializeComponent();

            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Group")
            {
                memberOf.MemberOf = ViewModel.Group.MemberOf;
                memberOf.PrimaryGroupId = ViewModel.Group.PrimaryGroupId;

                members.Members = ViewModel.Group.Members;
            }
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to delete this group?", "Delete", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
                ViewModel.DeleteCommand.Execute(ViewModel);
        }

        private async void MembersControl_Changed() //NOSONAR
        {
            await ViewModel.Get();
        }

        private async void MemberOfControl_Changed() //NOSONAR
        {
            await ViewModel.Get();
        }

        private void MemberOfControl_Error(string ErrorMessage) //NOSONAR
        {
            snackbarService.Show("Error",
                ErrorMessage,
                Wpf.Ui.Controls.ControlAppearance.Secondary,
                new Wpf.Ui.Controls.SymbolIcon(Wpf.Ui.Controls.SymbolRegular.ErrorCircle12),
                TimeSpan.FromSeconds(5)
            );
        }

    }
}
