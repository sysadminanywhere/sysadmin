using System.Threading.Tasks;

namespace SysAdmin.Services.Dialogs
{
    public interface IQuestionDialogService
    {
        Task<bool?> ShowDialog(object xamlRoot, string title, string message);
    }
}
