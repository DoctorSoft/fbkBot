using DataBase.Context;
using DataBase.QueriesAndCommands.Queries.Account;
using DataBase.QueriesAndCommands.Queries.Account.Models;
using Services.Core.Interfaces.ServiceTools;

namespace Services.ServiceTools
{
    public class AccountManager : IAccountManager
    {
        public AccountModel GetAccountById(long accountId)
        {
            return new GetAccountByIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByIdQuery
            {
                UserId = accountId
            });
        }
    }
}
