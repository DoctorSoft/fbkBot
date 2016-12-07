using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Commands.Accounts
{
    public class RecoverUserCommandHandler: ICommandHandler<RecoverUserCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext context;

        public RecoverUserCommandHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public VoidCommandResponse Handle(RecoverUserCommand command)
        {
            var account = context.Accounts.FirstOrDefault(model => model.Id == command.AccountId);

            account.IsDeleted = false;

            context.Accounts.AddOrUpdate(account);

            context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
