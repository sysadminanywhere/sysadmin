using SysAdmin.ActiveDirectory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysAdmin.Services.Dialogs
{
    public interface IAddContactDialogService
    {

        ContactEntry Contact { get; set; }

        string DistinguishedName { get; set; }

        Task<bool?> ShowDialog(string distinguishedName, object xamlRoot);
    }
}
