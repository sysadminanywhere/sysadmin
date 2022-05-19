using SysAdmin.ActiveDirectory.Models;
using System.Threading.Tasks;

namespace SysAdmin.Services.Dialogs
{
    public interface IResetPasswordDialog
    {

        UserEntry User { get; set; }

        string Password { get; set; }

        Task<bool?> ShowDialog(object xamlRoot);

    }
}
