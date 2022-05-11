using SysAdmin.ActiveDirectory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysAdmin.Services.Dialogs
{
    public interface IAddComputerDialogService
    {

        ComputerEntry Computer { get; set; }
        bool IsAccountEnabled { get; set; }

        string DistinguishedName { get; set; }

        Task<bool?> ShowDialog(string distinguishedName, object xamlRoot);
    }
}
