using SysAdmin.ActiveDirectory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysAdmin.Services.Dialogs
{
    public interface IAddUserDialogService
    {

        UserEntry User { get; set; }

        string Password { get; set; }
        bool IsCannotChangePassword { get; set; }
        bool IsPasswordNeverExpires { get; set; }
        bool IsAccountDisabled { get; set; }
        bool IsMustChangePassword { get; set; }

        Task<bool?> ShowDialog(object xamlRoot);
    }
}
