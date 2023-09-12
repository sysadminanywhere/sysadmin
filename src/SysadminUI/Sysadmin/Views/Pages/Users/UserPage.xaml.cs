using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Wpf.Ui.Common.Interfaces;
using Microsoft.Win32;
using System.IO;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class UserPage : INavigableView<ViewModels.UserViewModel>
    {
        public ViewModels.UserViewModel ViewModel
        {
            get;
        }

        public UserPage(ViewModels.UserViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure?", "Delete", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
                ViewModel.DeleteCommand.Execute(ViewModel);
        }

        private async void PhotoMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Multiselect = false;
                openFileDialog.Filter = "Image files (*.jpg;*.jpeg)|*.jpg;*.jpeg|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == true)
                {
                    await ViewModel.UpdatePhoto(ViewModel.User.DistinguishedName, File.ReadAllBytes(openFileDialog.FileName));
                    ShowPhoto();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void ShowPhoto()
        {
            if (ViewModel.User.JpegPhoto != null && ViewModel.User.JpegPhoto.Length > 0)
            {
                var image = new BitmapImage();
                using (var mem = new MemoryStream(ViewModel.User.JpegPhoto))
                {
                    mem.Position = 0;
                    image.BeginInit();
                    image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = null;
                    image.StreamSource = mem;
                    image.EndInit();
                }
                image.Freeze();
                personPicture.Source = image;
            }
        }

    }
}