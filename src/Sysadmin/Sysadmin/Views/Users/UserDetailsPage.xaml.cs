using LdapForNet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using SysAdmin.ActiveDirectory.Models;
using SysAdmin.ActiveDirectory.Services.Ldap;
using SysAdmin.Services;
using SysAdmin.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using static LdapForNet.Native.Native;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SysAdmin.Views.Users
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserDetailsPage : Page
    {

        public UsersViewModel ViewModel { get; } = new UsersViewModel();

        IBusyService busyService = App.Current.Services.GetService<IBusyService>();
        INotificationService notification = App.Current.Services.GetService<INotificationService>();

        public UserDetailsPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is UserEntry)
            {
                ViewModel.User = (UserEntry)e.Parameter;
                ShowPhoto();
            }
        }

        private async void MemberOfControl_Changed()
        {
            await ViewModel.Get(ViewModel.User.CN);
        }

        private async void PhotoBarButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.GetMainWindow());

                var openPicker = new Windows.Storage.Pickers.FileOpenPicker();
                openPicker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
                openPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
                openPicker.FileTypeFilter.Add(".jpg");
                openPicker.FileTypeFilter.Add(".jpeg");

                WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hwnd);
                StorageFile file = await openPicker.PickSingleFileAsync();
                if (file != null)
                {
                    var stream = await file.OpenStreamForReadAsync();
                    var bytes = new byte[(int)stream.Length];
                    stream.Read(bytes, 0, (int)stream.Length);

                    await UpdatePhoto(ViewModel.User.DistinguishedName, bytes);
                    await ViewModel.Get(ViewModel.User.CN);
                    ShowPhoto();
                }
            }
            catch (Exception ex)
            {
                notification.ShowErrorMessage(ex.Message);
            }
        }

        private async Task UpdatePhoto(string distinguishedName, byte[] photo)
        {
            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    var image = new DirectoryModificationAttribute
                    {
                        LdapModOperation = LdapModOperation.LDAP_MOD_REPLACE,
                        Name = "jpegPhoto"
                    };
                    image.Add(photo);
                    var response = await ldap.SendRequestAsync(new ModifyRequest(distinguishedName, image));
                }
            });

        }

        private void ShowPhoto()
        {
            if (ViewModel.User.JpegPhoto != null && ViewModel.User.JpegPhoto.Length > 0)
            {
                BitmapImage image = new BitmapImage();

                using (InMemoryRandomAccessStream ms = new InMemoryRandomAccessStream())
                {
                    using (DataWriter writer = new DataWriter(ms.GetOutputStreamAt(0)))
                    {
                        writer.WriteBytes((byte[])ViewModel.User.JpegPhoto);
                        writer.StoreAsync().GetResults();
                    }
                    image.SetSource(ms);
                }
                personPicture.ProfilePicture = image as ImageSource;
            }
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ADPropertiesPage), ViewModel.User.DistinguishedName);
        }
    }
}