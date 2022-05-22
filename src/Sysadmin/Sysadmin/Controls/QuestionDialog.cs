using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SysAdmin.Services.Dialogs;
using System;
using System.Threading.Tasks;

namespace SysAdmin.Controls
{
    public class QuestionDialog : IQuestionDialogService
    {
        public async Task<bool?> ShowDialog(string title, string message)
        {
            ContentDialog dialog = new ContentDialog();
            dialog.Title = title;
            dialog.PrimaryButtonText = "Yes";
            dialog.CloseButtonText = "No";
            dialog.DefaultButton = ContentDialogButton.Close;
            dialog.Content = message;

            dialog.XamlRoot = App.GetMainWindow().Content.XamlRoot;

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
                return true;
            else
                return false;

        }
    }
}