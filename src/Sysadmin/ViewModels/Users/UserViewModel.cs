using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Sysadmin.Services;
using SysAdmin.ActiveDirectory.Models;
using System.Threading.Tasks;
using System;
using Wpf.Ui;
using LdapForNet;
using SysAdmin.ActiveDirectory.Services.Ldap;
using SysAdmin.ActiveDirectory.Repositories;
using static LdapForNet.Native.Native;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Sysadmin.ViewModels
{
    public partial class UserViewModel : ViewModel
    {
        private bool _isInitialized = false;

        private INavigationService _navigationService;
        private IExchangeService _exchangeService;

        [ObservableProperty]
        private UserEntry _user = new UserEntry();

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        public UserViewModel(INavigationService navigationService, IExchangeService exchangeService)
        {
            _navigationService = navigationService;
            _exchangeService = exchangeService;
        }

        public override void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();

            if (_exchangeService.GetParameter() is UserEntry entry)
                User = entry;
        }

        private void InitializeViewModel()
        {
            _isInitialized = true;
        }

        [RelayCommand]
        private void OnClose()
        {
            _navigationService.Navigate(typeof(Views.Pages.UsersPage));
        }

        [RelayCommand]
        private void OnEdit()
        {
            _navigationService.Navigate(typeof(Views.Pages.EditUserPage));
        }

        [RelayCommand]
        private void OnOptions()
        {
            _navigationService.Navigate(typeof(Views.Pages.UserOptionsPage));
        }

        [RelayCommand]
        private void OnResetPassword()
        {
            _navigationService.Navigate(typeof(Views.Pages.ResetPasswordPage));
        }

        [RelayCommand]
        private async Task OnDelete()
        {
            try
            {
                await Delete(User);
                _navigationService.Navigate(typeof(Views.Pages.UsersPage));
            }
            catch (LdapException le)
            {
                ErrorMessage = SysAdmin.ActiveDirectory.LdapResult.GetErrorMessageFromResult(le.ResultCode);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        public async Task Delete(UserEntry user)
        {
            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    using (var usersRepository = new UsersRepository(ldap))
                    {
                        await usersRepository.DeleteAsync(user);
                    }
                }
            });
        }

        public async Task UpdatePhoto(string distinguishedName, byte[] photo)
        {
            try
            {
                await UpdatePhotoAsync(distinguishedName, photo);
                await UpdateThumbnailAsync(distinguishedName, photo);
            }
            catch (LdapException le)
            {
                ErrorMessage = SysAdmin.ActiveDirectory.LdapResult.GetErrorMessageFromResult(le.ResultCode);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

        }

        public async Task DeletePhoto(string distinguishedName)
        {
            try
            {
                await DeletePhotoAsync(distinguishedName);
                await DeleteThumbnailAsync(distinguishedName);
            }
            catch (LdapException le)
            {
                ErrorMessage = SysAdmin.ActiveDirectory.LdapResult.GetErrorMessageFromResult(le.ResultCode);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

        }

        private async Task UpdatePhotoAsync(string distinguishedName, byte[] photo)
        {

            byte[] jpegPhoto = Resize(photo, 648, 648);

            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    var image = new DirectoryModificationAttribute
                    {
                        LdapModOperation = LdapModOperation.LDAP_MOD_REPLACE,
                        Name = "jpegPhoto"
                    };
                    image.Add(jpegPhoto);
                    await ldap.SendRequestAsync(new ModifyRequest(distinguishedName, image));
                }
            });

        }

        private async Task UpdateThumbnailAsync(string distinguishedName, byte[] photo)
        {

            byte[] thumbnailPhoto = Resize(photo, 96, 96);

            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    var image = new DirectoryModificationAttribute
                    {
                        LdapModOperation = LdapModOperation.LDAP_MOD_REPLACE,
                        Name = "thumbnailPhoto"
                    };
                    image.Add(thumbnailPhoto);
                    await ldap.SendRequestAsync(new ModifyRequest(distinguishedName, image));
                }
            });

        }

        private async Task DeletePhotoAsync(string distinguishedName)
        {

            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    var image = new DirectoryModificationAttribute
                    {
                        LdapModOperation = LdapModOperation.LDAP_MOD_DELETE,
                        Name = "jpegPhoto"
                    };
                    await ldap.SendRequestAsync(new ModifyRequest(distinguishedName, image));
                }
            });

        }

        private async Task DeleteThumbnailAsync(string distinguishedName)
        {

            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    var image = new DirectoryModificationAttribute
                    {
                        LdapModOperation = LdapModOperation.LDAP_MOD_DELETE,
                        Name = "thumbnailPhoto"
                    };
                    await ldap.SendRequestAsync(new ModifyRequest(distinguishedName, image));
                }
            });

        }

        public async Task Get()
        {
            UserEntry entry = User;

            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    using (var usersRepository = new UsersRepository(ldap))
                    {
                        entry = await usersRepository.GetByCNAsync(User.CN);
                    }
                }
            });

            User = entry;
        }

        private Image byteArrayToImage(byte[] bytesArr)
        {
            using (MemoryStream memstr = new MemoryStream(bytesArr))
            {
                Image img = Image.FromStream(memstr);
                return img;
            }
        }

        private byte[] Resize(byte[] bytesArr, int width, int height)
        {
            return Resize(byteArrayToImage(bytesArr), width, height);
        }

        private byte[] Resize(Image image, int width, int height)
        {

            int drawWidth = width;
            int drawHeight = height;
            if (width != height)
            {
                drawWidth = Math.Min(width, height);
                drawHeight = drawWidth;
            }

            var destRect = new Rectangle((width - drawWidth) / 2, (height - drawHeight) / 2, drawWidth, drawHeight);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return ToByteArray(destImage, ImageFormat.Jpeg);
        }

        private byte[] ToByteArray(Image image, ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, format);
                return ms.ToArray();
            }
        }

    }
}