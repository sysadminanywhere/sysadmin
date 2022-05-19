using System.Threading.Tasks;

namespace SysAdmin.Services.Dialogs
{
    public interface IRenameDialogService
    {

        string CN { get; set; }

        string DistinguishedName { get; set; }

        Task<bool?> ShowDialog(string distinguishedName, string cn, object xamlRoot);

    }
}
