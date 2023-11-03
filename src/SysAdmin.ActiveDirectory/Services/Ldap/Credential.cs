namespace SysAdmin.ActiveDirectory.Services.Ldap
{
    public class Credential : ICredential
    {

        public Credential() { }

        public Credential(string username, string password)
        {
            UserName = username;
            Password = password;
        }

        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

    }
}
