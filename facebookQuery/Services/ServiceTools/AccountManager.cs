using System.Net;
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

        public WebProxy GetAccountProxy(AccountModel account)
        {
            return new WebProxy(account.Proxy)
            {
                Credentials = new NetworkCredential(account.ProxyLogin, account.ProxyPassword)
            };
        }

        public AccountModel GetAccountByFacebookId(long accountFacebookId)
        {
            return new GetAccountByFacebookIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByFacebookIdQuery()
            {
                FacebookUserId = accountFacebookId
            });
        }

        public string CreateHomePageUrl(long accountFacebookId)
        {
            return "https://www.facebook.com/profile.php?id=" + accountFacebookId;
        }

        public bool HasAWorkingProxy(long accountId)
        {
            var account = GetAccountById(accountId);

            return !account.ProxyDataIsFailed;
        }

        public bool HasAWorkingAuthorizationData(long accountId)
        {
            var account = GetAccountById(accountId);

            return !account.AuthorizationDataIsFailed;
        }
    }
}
