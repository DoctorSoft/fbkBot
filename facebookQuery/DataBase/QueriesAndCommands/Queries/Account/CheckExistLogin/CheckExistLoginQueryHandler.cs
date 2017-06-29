using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.Account.CheckExistLogin
{
    public class CheckExistLoginQueryHandler : IQueryHandler<CheckExistLoginQuery, bool>
    {
        private readonly DataBaseContext _context;

        public CheckExistLoginQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public bool Handle(CheckExistLoginQuery query)
        {
            var account = _context.Accounts.FirstOrDefault(model => model.Login == query.Login);

            return account != null;
        }
    }
}
