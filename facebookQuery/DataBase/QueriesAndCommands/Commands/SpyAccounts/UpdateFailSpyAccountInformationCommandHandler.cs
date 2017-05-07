using System.Data.Entity.Migrations;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Commands.SpyAccounts
{
    public class UpdateFailSpyAccountInformationCommandHandler : ICommandHandler<UpdateFailSpyAccountInformationCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext _context;

        public UpdateFailSpyAccountInformationCommandHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public VoidCommandResponse Handle(UpdateFailSpyAccountInformationCommand command)
        {
            var account = _context.SpyAccounts.FirstOrDefault(model => model.Id == command.AccountId);

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

                _context.SpyAccounts.AddOrUpdate(account);

                _context.SaveChanges();
            }

            return new VoidCommandResponse();
        }
    }
}
