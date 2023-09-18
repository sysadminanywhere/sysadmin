using FastReport.Export.Image;
using FastReport;
using System.Windows;
using Wpf.Ui.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.IO;
using System.Linq;
using FastReport.Export.PdfSimple;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Windows.Input;
using Wpf.Ui.Controls;
using System.Windows.Markup;
using System.Diagnostics;

namespace Sysadmin.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class ReportPage : INavigableView<ViewModels.ReportViewModel>
    {

        private Report report = new Report();
        private int currentPage = 0;
        private double imHeight;
        private double imWidth;

        public ViewModels.ReportViewModel ViewModel
        {
            get;
        }

        public ReportPage(ViewModels.ReportViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();

            imHeight = im.Height;
            imWidth = im.Width;

            this.Loaded += ReportPage_Loaded;
        }

        private async void ReportPage_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.IsBusy = true;
            stackMenu.IsEnabled = false;
            gridPreview.Visibility = Visibility.Collapsed;

            try
            {
                report = await ViewModel.Report.Report();

                SetContent(report);
                SetImage();

                stackMenu.IsEnabled = true;
                gridPreview.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                snackbar.Show("Error", ex.Message);
            }

            ViewModel.IsBusy = false;
        }

        private List<BitmapImage> pages = new List<BitmapImage>();

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
                if (value >= 0 && value < pages.Count)
                    currentPage = value;
            }
        }

        private void SetContent(Report report)
        {
            ImageExport exp = new ImageExport();

            exp.ImageFormat = ImageExportFormat.Png;
            exp.ResolutionX = 96;
            exp.ResolutionY = 96;

            pages.Clear();

            int n = report.PreparedPages.Count;

            for (int i = 1; i <= n; i++)
            {
                using (var ms = new MemoryStream())
                {
                    exp.PageRange = PageRange.PageNumbers;
                    exp.PageNumbers = i.ToString();

                    exp.Export(report, ms);

                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = ms;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    bitmap.Freeze();

                    pages.Add(bitmap);
                }
            }

            CurrentPage = 0;

            PageNumber.Minimum = 1;
            PageNumber.Maximum = pages.Count;
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
            CurrentPage = pages.Count - 1;
            SetImage();
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
                saveFileDialog.Filter = "Pdf file (*.pdf)|*.pdf|All files (*.*)|*.*";
                saveFileDialog.Title = "Save a PDF File";
                saveFileDialog.ShowDialog();

                if (!string.IsNullOrEmpty(saveFileDialog.FileName))
                {
                    PDFSimpleExport pdfExport = new PDFSimpleExport();
                    pdfExport.Export(report, saveFileDialog.FileName);

                    snackbarOk.Show();
                }
            }
            catch (Exception ex)
            {
                snackbar.Show("Error", ex.Message);
            }
        }

        private void PageNumber_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key < Key.D0 || e.Key > Key.D9)
            {
                e.Handled = true;
            }
        }

        private void PageNumber_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            NumberBox numberBox = (NumberBox)e.Source;
            if (!string.IsNullOrEmpty(numberBox.Text))
                CurrentPage = int.Parse(numberBox.Text);
        }
    }
}