using SysAdmin.ActiveDirectory.Models;
using System.Threading.Tasks;

namespace SysAdmin.Services.Dialogs
{
    public interface IAddContactDialogService
    {

        ContactEntry Contact { get; set; }

        string DistinguishedName { get; set; }

        Task<bool?> ShowDialog(string distinguishedName);
    }
}
