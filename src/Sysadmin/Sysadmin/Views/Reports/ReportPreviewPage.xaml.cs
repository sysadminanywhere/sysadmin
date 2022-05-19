using FastReport;
using FastReport.Export.Image;
using FastReport.Export.PdfSimple;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using SysAdmin.Services;
using SysAdmin.Services.Reports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SysAdmin.Views.Reports
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ReportPreviewPage : Page
    {
        public string Title { get; private set; }
        public string Description { get; private set; }

        IBusyService busyService = App.Current.Services.GetService<IBusyService>();
        INotificationService notification = App.Current.Services.GetService<INotificationService>();

        private Report report;
        private ImageExport ex;
        private bool hasMultipleFiles;
        private int currentPage = 0;
        private double imHeight;
        private double imWidth;

        Windows.Storage.StorageFolder temporaryFolder = Windows.Storage.ApplicationData.Current.TemporaryFolder;
        public ReportPreviewPage()
        {
            this.InitializeComponent();
            imHeight = im.Height;
            imWidth = im.Width;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is IReport)
            {
                IReport r = (IReport)e.Parameter;

                busyService.Busy();

                try
                {
                    Title = r.Name;
                    Description = r.Description;
                    report = await r.Report();

                    ex = new ImageExport();
                    ex.HasMultipleFiles = HasMultipleFiles;
                    SetContent(report);
                    SetImage();
                }
                catch (Exception ex)
                {
                    notification.ShowErrorMessage(ex.Message);
                }

                busyService.Idle();
            }
        }

        public List<BitmapImage> pages = new List<BitmapImage>();

        public void SetImage()
        {
            im.Source = pages[CurrentPage];
            im.Height = imHeight;
            im.Width = imWidth;
            PageNumber.Text = (CurrentPage + 1).ToString();
        }

        public int CurrentPage
        {
            get { return currentPage; }
            set
            {
                if (value >= 0 && value < pages.Count())
                    currentPage = value;
            }
        }
        public bool HasMultipleFiles
        {
            get { return hasMultipleFiles; }
            set
            {
                hasMultipleFiles = value;
                ex.HasMultipleFiles = value;
            }
        }

        private void SetContent(Report report)
        {
            DeleteTempFiles();
            ex.ImageFormat = ImageExportFormat.Png;
            ex.ResolutionX = 96;
            ex.ResolutionY = 96;
            Random rnd = new Random();
            ex.Export(report, temporaryFolder.Path + "/test." + rnd.Next(100) + ".png");
            foreach (string file in ex.GeneratedFiles)
            {
                BitmapImage image = new BitmapImage(new Uri(file));
                pages.Add(image);
            }
            CurrentPage = 0;
        }


        public void DeleteTempFiles()
        {
            FileInfo[] path = new DirectoryInfo(temporaryFolder.Path).GetFiles("*test*", SearchOption.AllDirectories);
            pages.Clear();
            foreach (FileInfo file in path)
            {
                File.Delete(file.FullName);
            }
        }

        private void Zoom_in_Click(object sender, RoutedEventArgs e)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                if (ex != null)
                {
                    ex.ImageFormat = ImageExportFormat.Png;
                    ex.Resolution += 25;
                    ex.PageNumbers = (CurrentPage + 1).ToString();
                    report.Export(ex, stream);

                    if (CurrentPage >= 0 && CurrentPage < pages.Count())
                        im.Source = LoadImage(stream);
                    PageNumber.Text = (CurrentPage + 1).ToString();
                }
            }
            im.Width += 50;
            im.Height += 50;
        }

        private static BitmapImage LoadImage(Stream stream)
        {
            var image = new BitmapImage();

            image.SetSource(stream.AsRandomAccessStream());

            return image;
        }

        private void First_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = 0;
            SetImage();
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage--;
            SetImage();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage++;
            SetImage();
        }

        private void Last_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = pages.Count() - 1;
            SetImage();
        }

        private void Zoom_out_Click(object sender, RoutedEventArgs e)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                if (ex != null)
                {
                    ex.ImageFormat = ImageExportFormat.Png;
                    ex.PageNumbers = (CurrentPage + 1).ToString();
                    report.Export(ex, stream);

                    if (CurrentPage >= 0 && CurrentPage < pages.Count())
                        im.Source = LoadImage(stream);
                    PageNumber.Text = (CurrentPage + 1).ToString();
                }
            }
            im.Width -= 50;
            im.Height -= 50;
        }

        private void PageNumber_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                if (int.Parse(PageNumber.Text) > 0)
                {
                    CurrentPage = int.Parse(PageNumber.Text) - 1;
                    SetImage();
                }
            }
        }

        private async void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.GetMainWindow());

                var savePicker = new Windows.Storage.Pickers.FileSavePicker();
                savePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
                savePicker.FileTypeChoices.Add("Pdf file", new List<string>() { ".pdf" });
                savePicker.SuggestedFileName = Title + ".pdf";

                WinRT.Interop.InitializeWithWindow.Initialize(savePicker, hwnd);
                StorageFile file = await savePicker.PickSaveFileAsync();
                if (file != null)
                {
                    PDFSimpleExport pdfExport = new PDFSimpleExport();
                    pdfExport.Export(report, file.Path);

                    notification.ShowSuccessMessage("Report successfuly exported");
                }
            }
            catch (Exception ex)
            {
                notification.ShowErrorMessage(ex.Message);
            }
        }

    }
}