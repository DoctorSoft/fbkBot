using System.Collections.Generic;
using System.Net;
using DataBase.QueriesAndCommands.Queries.Account.Models;

namespace Services.Interfaces.ServiceTools
{
    public interface IAccountManager
    {
        AccountModel GetAccountById(long accountId);

        WebProxy GetAccountProxy(AccountModel account);

        AccountModel GetAccountByFacebookId(long accountFacebookId);

        List<AccountModel> GetWorkAccounts();

        string CreateHomePageUrl(long accountFacebookId);
    }
}
