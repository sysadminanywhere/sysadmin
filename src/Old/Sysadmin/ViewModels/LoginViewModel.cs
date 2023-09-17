using SysAdmin.ActiveDirectory.Services.Ldap;
using System.Threading.Tasks;

namespace SysAdmin.ViewModels
{
    public class LoginViewModel
    {

        public async Task<bool> Login()
        {
            bool isConnected = false;

            await Task.Run(() =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    isConnected = ldap.IsConnected;
                }
            });

            return isConnected;
        }
    }
}