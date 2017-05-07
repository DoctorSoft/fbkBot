using System.Data.Entity.Migrations;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Commands.SpyAccounts
{
    public class DeleteSpyAccounCommandHandler : ICommandHandler<DeleteSpyAccounCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext _context;

        public DeleteSpyAccounCommandHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public VoidCommandResponse Handle(DeleteSpyAccounCommand command)
        {
            var account = _context.SpyAccounts.FirstOrDefault(model => model.Id == command.AccountId);

            if (account != null)
            {
                account.IsDeleted = true;

                _context.SpyAccounts.AddOrUpdate(account);
            }

            _context.SaveChanges();

            return new VoidCommandResponse();
        }
    }
}
