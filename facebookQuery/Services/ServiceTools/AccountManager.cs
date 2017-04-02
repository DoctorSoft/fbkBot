using System.Collections.Generic;
using System.Net;
using DataBase.Context;
using DataBase.QueriesAndCommands.Queries.Account;
using DataBase.QueriesAndCommands.Queries.Account.GetWorkAccounts;
using DataBase.QueriesAndCommands.Queries.Account.Models;
using DataBase.QueriesAndCommands.Queries.AccountInformation;
using Services.Interfaces.ServiceTools;
using Services.ViewModels.AccountInformationModels;

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
        public AccountInformationViewModel GetAccountInformation(long accountId)
        {
            var accountInformationModel = new GetAccountInformationQueryHandler(new DataBaseContext()).Handle(new GetAccountInformationQuery
            {
                AccountId = accountId
            });

            if (accountInformationModel == null)
            {
                return new AccountInformationViewModel();
            }

            var result = new AccountInformationViewModel
            {
                CountCurrentFriends = accountInformationModel.AccountInformationData != null ? accountInformationModel.AccountInformationData.CountCurrentFriends : 0,
                CountNewMessages = accountInformationModel.AccountInformationData!=null ? accountInformationModel.AccountInformationData.CountNewMessages : 0,
                CountIncommingFriendsRequest = accountInformationModel.AccountInformationData != null ? accountInformationModel.AccountInformationData.CountIncommingFriendsRequest : 0
            };

            return result;
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
            return new GetAccountByFacebookIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByFacebookIdQuery
            {
                FacebookUserId = accountFacebookId
            });
        }

        public List<AccountModel> GetWorkAccounts()
        {
            return new GetWorkAccountsQueryHandler(new DataBaseContext()).Handle(new GetWorkAccountsQuery());
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
