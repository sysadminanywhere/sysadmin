using SysAdmin.ActiveDirectory.Models;
using System.Threading.Tasks;

namespace SysAdmin.Services.Dialogs
{
    public interface IEditContactDialogService
    {

        ContactEntry Contact { get; set; }

        Task<bool?> ShowDialog();
    }
}
