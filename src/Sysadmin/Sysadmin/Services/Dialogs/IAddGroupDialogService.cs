using SysAdmin.ActiveDirectory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysAdmin.Services.Dialogs
{
    public interface IAddGroupDialogService
    {

        GroupEntry Group { get; set; }
        GroupScopes GroupScope { get; set; }
        bool IsSecurity { get; set; }


        Task<bool?> ShowDialog(object xamlRoot);
    }
}
