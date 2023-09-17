namespace SysAdmin.ActiveDirectory.Services.Ldap
{
    public interface IServer
    {
        string ServerName { get; set; }
        int Port { get; set; }
        bool IsSSL { get; set; }
    }
}