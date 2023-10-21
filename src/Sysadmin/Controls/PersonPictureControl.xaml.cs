using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Sysadmin.Controls
{
    /// <summary>
    /// Interaction logic for PersonPictureControl.xaml
    /// </summary>
    public partial class PersonPictureControl : UserControl
    {

        public static readonly DependencyProperty DisplayNameProperty = DependencyProperty.Register(
            "DisplayName", typeof(string),
            typeof(PersonPictureControl)
            );

        public string DisplayName
        {
            get => (string)GetValue(DisplayNameProperty);
            set
            {
                SetValue(DisplayNameProperty, value);
                initials.Text = GetInitials(value);
            }
        }

        public static readonly DependencyProperty ProfilePictureProperty = DependencyProperty.Register(
            "ProfilePicture", typeof(ImageSource),
            typeof(PersonPictureControl)
            );

        public ImageSource ProfilePicture
        {
            get => (ImageSource)GetValue(ProfilePictureProperty);
            set
            {
                SetValue(ProfilePictureProperty, value);
                pictureImage.Source = value;
            }
        }

        private byte[] jpegPhoto;

        public byte[] JpegPhoto
        {
            get { return jpegPhoto; }
            set
            {
                jpegPhoto = value;
                ShowPhoto();
            }
        }

        public PersonPictureControl()
        {
            InitializeComponent();
        }

        private string GetInitials(string name)
        {

            if (string.IsNullOrEmpty(name))
                return string.Empty;

            string[] nameSplit = name.Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries);

            string initials = "";

            foreach (string item in nameSplit)
            {
                initials += item.Substring(0, 1).ToUpper();
            }

            return initials;
        }

        private void ShowPhoto()
        {
            if (JpegPhoto != null && JpegPhoto.Length > 0)
            {
                var image = new BitmapImage();
                using (var mem = new MemoryStream(JpegPhoto))
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
                pictureImage.Source = image;
            }
            else
            {
                pictureImage.Source = null;
            }
        }

    }
}