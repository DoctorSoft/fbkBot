using DataBase.QueriesAndCommands.Queries.Account.Models;

namespace Services.Core.Interfaces.ServiceTools
{
    public interface IAccountManager
    {
        AccountModel GetAccountById(long accountId);

        AccountModel GetAccountByFacebookId(long accountFacebookId);
    }
}
