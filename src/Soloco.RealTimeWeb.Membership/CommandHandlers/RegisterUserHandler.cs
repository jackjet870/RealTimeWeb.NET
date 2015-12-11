using System;
using System.Threading.Tasks;
using Marten;
using Microsoft.AspNet.Identity;
using Soloco.RealTimeWeb.Common.Infrastructure;
using Soloco.RealTimeWeb.Common.Infrastructure.Messages;
using Soloco.RealTimeWeb.Membership.Domain;
using Soloco.RealTimeWeb.Membership.Messages.Commands;

namespace Soloco.RealTimeWeb.Membership.CommandHandlers
{
    public class RegisterUserHandler : CommandHandler<RegisterUserCommand>
    {
        private readonly UserManager<User> _userManager;

        public RegisterUserHandler(UserManager<User> userManager, IDocumentSession session, IDisposable scope) : base(session, scope)
        {
            _userManager = userManager;
        }

        protected override async Task<CommandResult> Execute(RegisterUserCommand command)
        {
            var user = new User(command.UserName, command.EMail);
            var result = await _userManager.CreateAsync(user, command.Password);
            return result.ToCommandResult();
        }
    }
}