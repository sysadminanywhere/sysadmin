using LdapForNet.Native;

namespace SysAdmin.ActiveDirectory.Services.Ldap
{
    public class SecureServer : IServer
    {

        public SecureServer() { }

        public SecureServer(string server)
        {
            ServerName = server;
        }

        public string ServerName { get; set; } = string.Empty;
        public int Port { get; set; } = 636;
        public bool IsSSL { get; set; } = true;
    }
}
