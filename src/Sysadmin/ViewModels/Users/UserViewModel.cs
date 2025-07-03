using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LdapForNet;
using Sysadmin.Services;
using SysAdmin.ActiveDirectory;
using SysAdmin.ActiveDirectory.Models;
using SysAdmin.ActiveDirectory.Repositories;
using SysAdmin.ActiveDirectory.Services.Ldap;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Wpf.Ui;
using Wpf.Ui.Controls;
using static LdapForNet.Native.Native;
using Image = System.Drawing.Image;

namespace Sysadmin.ViewModels
{
    public partial class UserViewModel : ViewModel
    {
        private bool isInitialized = false;

        private INavigationService navigationService;
        private IExchangeService exchangeService;
        private ISnackbarService snackbarService;

        [ObservableProperty]
        private UserEntry _user = new UserEntry();

        public UserViewModel(INavigationService navigationService, IExchangeService exchangeService, ISnackbarService snackbarService)
        {
            this.navigationService = navigationService;
            this.exchangeService = exchangeService;
            this.snackbarService = snackbarService;
        }

        public override void OnNavigatedTo()
        {
            if (!isInitialized)
                InitializeViewModel();

            if (exchangeService.GetParameter() is UserEntry entry)
                User = entry;
        }

        private void InitializeViewModel()
        {
            isInitialized = true;
        }

        [RelayCommand]
        private void OnClose()
        {
            navigationService.Navigate(typeof(Views.Pages.UsersPage));
        }

        [RelayCommand]
        private void OnEdit()
        {
            navigationService.Navigate(typeof(Views.Pages.EditUserPage));
        }

        [RelayCommand]
        private void OnOptions()
        {
            navigationService.Navigate(typeof(Views.Pages.UserOptionsPage));
        }

        [RelayCommand]
        private void OnResetPassword()
        {
            navigationService.Navigate(typeof(Views.Pages.ResetPasswordPage));
        }

        [RelayCommand]
        private async Task OnDelete()
        {
            try
            {
                await Delete(User);
                navigationService.Navigate(typeof(Views.Pages.UsersPage));
            }
            catch (LdapException le)
            {
                snackbarService.Show("Error",
                    LdapResult.GetErrorMessageFromResult(le.ResultCode),
                    ControlAppearance.Secondary,
                    new SymbolIcon(SymbolRegular.ErrorCircle12),
                    TimeSpan.FromSeconds(5)
                );
            }
            catch (Exception ex)
            {
                snackbarService.Show("Error",
                    ex.Message,
                    ControlAppearance.Secondary,
                    new SymbolIcon(SymbolRegular.ErrorCircle12),
                    TimeSpan.FromSeconds(5)
                );
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
                snackbarService.Show("Error",
                    LdapResult.GetErrorMessageFromResult(le.ResultCode),
                    ControlAppearance.Secondary,
                    new SymbolIcon(SymbolRegular.ErrorCircle12),
                    TimeSpan.FromSeconds(5)
                );
            }
            catch (Exception ex)
            {
                snackbarService.Show("Error",
                    ex.Message,
                    ControlAppearance.Secondary,
                    new SymbolIcon(SymbolRegular.ErrorCircle12),
                    TimeSpan.FromSeconds(5)
                );
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
                snackbarService.Show("Error",
                    LdapResult.GetErrorMessageFromResult(le.ResultCode),
                    ControlAppearance.Secondary,
                    new SymbolIcon(SymbolRegular.ErrorCircle12),
                    TimeSpan.FromSeconds(5)
                );
            }
            catch (Exception ex)
            {
                snackbarService.Show("Error",
                    ex.Message,
                    ControlAppearance.Secondary,
                    new SymbolIcon(SymbolRegular.ErrorCircle12),
                    TimeSpan.FromSeconds(5)
                );
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