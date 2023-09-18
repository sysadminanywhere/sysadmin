using SysAdmin.ActiveDirectory.Models;
using System.Threading.Tasks;

namespace SysAdmin.Services.Dialogs
{
    public interface IEditComputerDialogService
    {

        ComputerEntry Computer { get; set; }

        Task<bool?> ShowDialog();
    }
}
