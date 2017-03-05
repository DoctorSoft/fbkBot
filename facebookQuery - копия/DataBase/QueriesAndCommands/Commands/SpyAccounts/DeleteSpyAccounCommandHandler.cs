using System.Data.Entity.Migrations;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Commands.SpyAccounts
{
    public class DeleteSpyAccounCommandHandler : ICommandHandler<DeleteSpyAccounCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext context;

        public DeleteSpyAccounCommandHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public VoidCommandResponse Handle(DeleteSpyAccounCommand command)
        {
            var account = context.SpyAccounts.FirstOrDefault(model => model.Id == command.AccountId);

            account.IsDeleted = true;

            context.SpyAccounts.AddOrUpdate(account);

            context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
