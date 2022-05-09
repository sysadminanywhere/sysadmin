namespace SysAdmin.ActiveDirectory.Services.Ldap
{
    public class Credential : ICredential
    {

        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

    }
}
