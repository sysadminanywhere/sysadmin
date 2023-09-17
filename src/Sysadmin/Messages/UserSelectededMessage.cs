using CommunityToolkit.Mvvm.Messaging.Messages;
using SysAdmin.ActiveDirectory.Models;

namespace Sysadmin.Messages
{
    public class UserSelectededMessage : ValueChangedMessage<UserEntry>
    {
        public UserSelectededMessage(UserEntry value) : base(value)
        {
        }
    }
}
