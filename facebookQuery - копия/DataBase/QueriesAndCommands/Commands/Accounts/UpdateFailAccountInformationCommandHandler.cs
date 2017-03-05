using System.Data.Entity.Migrations;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Commands.Accounts
{
    public class UpdateFailAccountInformationCommandHandler : ICommandHandler<UpdateFailAccountInformationCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext context;

        public UpdateFailAccountInformationCommandHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public VoidCommandResponse Handle(UpdateFailAccountInformationCommand command)
        {
            var account = context.Accounts.FirstOrDefault(model => model.Id == command.AccountId);

            if (account != null)
            {
                if (command.AuthorizationDataIsFailed != null)
                {
                    account.AuthorizationDataIsFailed = (bool)command.AuthorizationDataIsFailed;
                }

                if (command.ProxyDataIsFailed != null)
                {
                    account.ProxyDataIsFailed = (bool)command.ProxyDataIsFailed;
                }

                context.Accounts.AddOrUpdate(account);

                context.SaveChanges();
            }

            return new VoidCommandResponse();
        }
    }
}
