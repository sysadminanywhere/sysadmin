using SysAdmin.ActiveDirectory.Models;
using System.Threading.Tasks;

namespace SysAdmin.Services.Dialogs
{
    public interface IUserOptionsDialogService
    {

        UserEntry User { get; set; }

        bool IsCannotChangePassword { get; set; }
        bool IsPasswordNeverExpires { get; set; }
        bool IsAccountDisabled { get; set; }
        bool IsMustChangePassword { get; set; }

        Task<bool?> ShowDialog(object xamlRoot);
    }
}
