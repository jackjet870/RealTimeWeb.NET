using Soloco.RealTimeWeb.Common.Infrastructure;
using Soloco.RealTimeWeb.Common.Infrastructure.Messages;

namespace Soloco.RealTimeWeb.Membership.Messages.Commands
{
    public class RegisterUserCommand : IMessage<CommandResult>
    {
        public string UserName { get; }
        public string EMail { get; set; }
        public string Password { get; }

        public RegisterUserCommand(string userName, string eMail, string password)
        {
            UserName = userName;
            EMail = eMail;
            Password = password;
        }
    }
}
