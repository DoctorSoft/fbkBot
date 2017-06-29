using System.Collections.Generic;
using System.Linq;
using System.Net;
using DataBase.Context;
using DataBase.QueriesAndCommands.Queries.Account;
using DataBase.QueriesAndCommands.Queries.Account.GetWorkAccounts;
using DataBase.QueriesAndCommands.Queries.Account.Models;
using DataBase.QueriesAndCommands.Queries.AccountInformation;
using Services.Interfaces.ServiceTools;
using Services.ViewModels.AccountInformationModels;
using Services.ViewModels.HomeModels;

namespace Services.ServiceTools
{
    public class AccountManager : IAccountManager
    {
        public AccountViewModel GetAccountById(long accountId)
        {
            var accountModel = new GetAccountByIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByIdQuery
            {
                UserId = accountId
            });

            if (accountModel == null)
            {
                return null;
            }

            var accountViewModel = new AccountViewModel
            {
                Id = accountModel.Id,
                Login = accountModel.Login,
                Proxy = accountModel.Proxy,
                FacebookId = accountModel.FacebookId,
                ProxyLogin = accountModel.ProxyLogin,
                Name = accountModel.Name,
                ProxyPassword = accountModel.ProxyPassword,
                ConformationDataIsFailed = accountModel.ConformationIsFailed,
                AuthorizationDataIsFailed = accountModel.AuthorizationDataIsFailed,
                ProxyDataIsFailed = accountModel.ProxyDataIsFailed,
                Cookie = accountModel.Cookie != null ? accountModel.Cookie.CookieString : "",
                PageUrl = accountModel.PageUrl,
                Password = accountModel.Password,
                UserAgentId = accountModel.UserAgentId,
                GroupSettingsId = accountModel.GroupSettingsId,
                IsDeleted = accountModel.IsDeleted
            };

            return accountViewModel;
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

        public WebProxy GetAccountProxy(AccountViewModel account)
        {
            return new WebProxy(account.Proxy)
            {
                Credentials = new NetworkCredential(account.ProxyLogin, account.ProxyPassword)
            };
        }

        public AccountViewModel GetAccountByFacebookId(long accountFacebookId)
        {
            var accountModel = new GetAccountByFacebookIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByFacebookIdQuery
            {
                FacebookUserId = accountFacebookId
            });
            var accountViewModel = new AccountViewModel
            {
                Id = accountModel.Id,
                Login = accountModel.Login,
                Proxy = accountModel.Proxy,
                FacebookId = accountModel.FacebookId,
                ProxyLogin = accountModel.ProxyLogin,
                Name = accountModel.Name,
                ProxyPassword = accountModel.ProxyPassword,
                ConformationDataIsFailed = accountModel.ConformationIsFailed,
                AuthorizationDataIsFailed = accountModel.AuthorizationDataIsFailed,
                ProxyDataIsFailed = accountModel.ProxyDataIsFailed,
                Cookie = accountModel.Cookie != null ? accountModel.Cookie.CookieString : "",
                PageUrl = accountModel.PageUrl,
                Password = accountModel.Password,
                UserAgentId = accountModel.UserAgentId,
                GroupSettingsId = accountModel.GroupSettingsId,
                IsDeleted = accountModel.IsDeleted
            };

            return accountViewModel;
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
        public bool HasAWorkingAccount(long accountId)
        {
            var account = GetAccountById(accountId);

            return !account.AuthorizationDataIsFailed && !account.ProxyDataIsFailed && !account.ConformationDataIsFailed;
        }

        public List<AccountDataViewModel> SortAccountsByWorkStatus(List<AccountDataViewModel> accounts)
        {
            var result = accounts.OrderByDescending(model => !model.Account.ProxyDataIsFailed && !model.Account.AuthorizationDataIsFailed && !model.Account.ConformationDataIsFailed).ToList();
            return result;
        }
    }
}
