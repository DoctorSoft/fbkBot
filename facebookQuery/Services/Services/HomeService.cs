using System.Collections.Generic;
using System.Linq;
using Constants;
using Constants.EnumExtension;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.Accounts;
using DataBase.QueriesAndCommands.Commands.Cookies;
using DataBase.QueriesAndCommands.Queries.Account;
using Engines.Engines.GetNewCookiesEngine;
using Engines.Engines.GetNewNoticesEngine;
using RequestsHelpers;
using Services.ViewModels.AccountModels;
using Services.ViewModels.HomeModels;

namespace Services.Services
{
    public class HomeService
    {
        public List<AccountViewModel> GetAccounts()
        {
            var accounts = new GetAccountsQueryHandler(new DataBaseContext()).Handle(new GetAccountsQuery
            {
                Count = 10,
                Page = 0
            });

            return accounts.Select(model => new AccountViewModel
            {
                Id = model.Id,
                PageUrl = model.PageUrl,
                Login = model.Login,
                Password = model.Password,
                FacebookId = model.UserId
            }).ToList();
        }


        public bool RefreshCookies(long accountId, string login, string password)
        {
            var newCookie = new GetNewCookiesEngine().Execute(new GetNewCookiesModel()
            {
                Login = login,
                Password = password
            }).CookiesString;

            new UpdateCookiesHandler(new DataBaseContext()).Handle(new UpdateCookiesCommand()
            {
                AccountId = accountId,
                NewCookieString = newCookie
            });

            return true;
        }

        public GetNewNoticesResponseModel GetNewNotices(long accountId)
        {
            var account = new GetAccountByFacebookIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByFacebookIdQuery
            {
                UserId = accountId
            });
            var statusModel = new GetNewNoticesEngine().Execute(new GetNewNoticesModel()
            {
                ResponsePage = RequestsHelper.Get(Urls.HomePage.GetDiscription(), account.Cookie.CookieString) 
            });

            return statusModel;
        }

        public AccountActionModel GetAccountByUserId(long? userId)
        {
            if (userId == null)
            {
                return new AccountActionModel();
            }

            var account = new GetAccountByFacebookIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByFacebookIdQuery
            {
                UserId = userId.Value
            });

            return new AccountActionModel
            {
                Id = account.Id,
                Login = account.Login,
                Password = account.Password,
                UserId = account.UserId,
                PageUrl = account.PageUrl,
                Name = account.Name,
                FacebookId = account.FacebookId
            };
        }

        public AccountDraftViewModel GetAccountById(long? userId)
        {
            if (userId == null)
            {
                return new AccountDraftViewModel();
            }

            var account = new GetAccountByIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByIdQuery
            {
                UserId = userId.Value
            });

            return new AccountDraftViewModel
            {
                Id = account.Id,
                Login = account.Login,
                Password = account.Password,
                PageUrl = account.PageUrl,
                Name = account.Name,
                FacebookId = account.FacebookId
            };
        }

        public long AddOrUpdateAccount(AccountDraftViewModel model)
        {
            var accountId = new AddOrUpdateAccountCommandHandler(new DataBaseContext()).Handle(new AddOrUpdateAccountCommand
            {
                Id = model.Id,
                Name = model.Name,
                PageUrl = model.PageUrl,
                FacebookId = model.FacebookId,
                Password = model.Password,
                Login = model.Login
            });

            return accountId;
        }

        public CookiesViewModel GetAccountCookies(long accountId)
        {
            var account = new GetAccountByIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByIdQuery
            {
                UserId = accountId
            });

            return new CookiesViewModel
            {
                AccountId = accountId,
                Value = account.Cookie.CookieString
            };
        }

        public void UpdateCookies(CookiesViewModel model)
        {
            new UpdateCookiesHandler(new DataBaseContext()).Handle(new UpdateCookiesCommand
            {
                AccountId = model.AccountId,
                NewCookieString = model.Value
            });
        }
    }
}


