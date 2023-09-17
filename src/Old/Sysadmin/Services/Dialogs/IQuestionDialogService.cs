using System.Threading.Tasks;

namespace SysAdmin.Services.Dialogs
{
    public interface IQuestionDialogService
    {
        Task<bool?> ShowDialog(string title, string message);
    }
}
