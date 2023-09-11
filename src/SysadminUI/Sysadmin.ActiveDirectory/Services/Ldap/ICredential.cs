namespace SysAdmin.ActiveDirectory.Services.Ldap
{
    public interface ICredential
    {
        string UserName { get; set; }
        string Password { get; set; }

    }
}
