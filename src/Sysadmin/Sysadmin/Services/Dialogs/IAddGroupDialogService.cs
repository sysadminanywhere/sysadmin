using SysAdmin.ActiveDirectory.Models;
using System.Threading.Tasks;

namespace SysAdmin.Services.Dialogs
{
    public interface IAddGroupDialogService
    {

        GroupEntry Group { get; set; }
        GroupScopes GroupScope { get; set; }
        bool IsSecurity { get; set; }

        string DistinguishedName { get; set; }


        Task<bool?> ShowDialog(string distinguishedName, object xamlRoot);
    }
}
