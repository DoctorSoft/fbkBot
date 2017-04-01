using System.Data.Entity.Migrations;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Commands.Accounts
{
    public class UpdateFailAccountInformationCommandHandler : ICommandHandler<UpdateFailAccountInformationCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext _context;

        public UpdateFailAccountInformationCommandHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public VoidCommandResponse Handle(UpdateFailAccountInformationCommand command)
        {
            var account = _context.Accounts.FirstOrDefault(model => model.Id == command.AccountId);

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

                if (command.ConformationIsFailed != null)
                {
                    account.ConformationIsFailed = (bool)command.ConformationIsFailed;
                }

                _context.Accounts.AddOrUpdate(account);

                _context.SaveChanges();
            }

            return new VoidCommandResponse();
        }
    }
}
