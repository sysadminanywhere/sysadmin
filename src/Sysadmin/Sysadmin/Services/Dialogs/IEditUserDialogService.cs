using SysAdmin.ActiveDirectory.Models;
using System.Threading.Tasks;

namespace SysAdmin.Services.Dialogs
{
    public interface IEditUserDialogService
    {

        UserEntry User { get; set; }

        Task<bool?> ShowDialog(object xamlRoot);
    }
}
