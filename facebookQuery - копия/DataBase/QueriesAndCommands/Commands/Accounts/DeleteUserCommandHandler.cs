using System.Data.Entity.Migrations;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Commands.Accounts
{
    public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext context;

        public DeleteUserCommandHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public VoidCommandResponse Handle(DeleteUserCommand command)
        {
            var account = context.Accounts.FirstOrDefault(model => model.Id == command.AccountId);

            account.IsDeleted = true;

            context.Accounts.AddOrUpdate(account);

            context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
