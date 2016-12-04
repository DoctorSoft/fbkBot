using System.Net;
using DataBase.QueriesAndCommands.Queries.Account.Models;

namespace Services.Core.Interfaces.ServiceTools
{
    public interface IAccountManager
    {
        AccountModel GetAccountById(long accountId);

        WebProxy GetAccountProxy(AccountModel account);

        AccountModel GetAccountByFacebookId(long accountFacebookId);
    }
}
