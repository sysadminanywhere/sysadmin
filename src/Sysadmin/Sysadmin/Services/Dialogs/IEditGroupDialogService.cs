using SysAdmin.ActiveDirectory.Models;
using System.Threading.Tasks;

namespace SysAdmin.Services.Dialogs
{
    public interface IEditGroupDialogService
    {

        GroupEntry Group { get; set; }

        Task<bool?> ShowDialog();
    }
}
