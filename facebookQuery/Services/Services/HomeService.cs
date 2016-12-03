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
using OpenQA.Selenium.PhantomJS;
using RequestsHelpers;
using Services.Core.Interfaces.ServiceTools;
using Services.ServiceTools;
using Services.ViewModels.AccountModels;
using Services.ViewModels.HomeModels;

namespace Services.Services
{
    public class HomeService
    {
        private IAccountManager _accountManager;

        public HomeService()
        {
            _accountManager = new AccountManager();    
        }

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
                FacebookId = model.UserId,
                Proxy = model.Proxy,
                ProxyLogin = model.ProxyLogin,
                ProxyPassword = model.ProxyPassword
            }).ToList();
        }

        public PhantomJSDriver RegisterNewDriver(AccountViewModel account)
        {
            if (string.IsNullOrWhiteSpace(account.Proxy))
            {
                return new PhantomJSDriver();
            }

            var service = PhantomJSDriverService.CreateDefaultService();
            service.AddArgument(string.Format("--proxy-auth={0}:{1}", account.ProxyLogin, account.ProxyPassword));
            service.AddArgument(string.Format("--proxy={0}", account.Proxy));

            var driver = new PhantomJSDriver(service);

            return driver;
        }

        public bool RefreshCookies(AccountViewModel account)
        {
            var driver = RegisterNewDriver(account);
            var newCookie = new GetNewCookiesEngine().Execute(new GetNewCookiesModel()
            {
                Login = account.Login,
                Password = account.Password,
                Driver = driver
            }).CookiesString;

            new UpdateCookiesHandler(new DataBaseContext()).Handle(new UpdateCookiesCommand()
            {
                AccountId = account.Id,
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
                FacebookId = account.FacebookId,
                Proxy = account.Proxy,
                ProxyLogin = account.ProxyLogin,
                ProxyPassword = account.ProxyPassword
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
                Login = model.Login,
                Proxy = model.Proxy,
                ProxyLogin = model.ProxyLogin,
                ProxyPassword = model.ProxyPassword
            });

            RefreshCookies(new AccountViewModel
            {
                Id = model.Id.Value,
                Login = model.Login,
                Password = model.Password,
                Proxy = model.Proxy,
                ProxyLogin = model.ProxyLogin,
                ProxyPassword = model.ProxyPassword,
            });

            return accountId;
        }

        public CookiesViewModel GetAccountCookies(long accountId)
        {
            var account = new GetAccountByIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByIdQuery
            {
                UserId = accountId
            });

            if (account.Cookie == null)
            {
                return new CookiesViewModel
                {
                    AccountId = accountId,
                    Value = "Cookie is not created"
                };
            }
            return new CookiesViewModel
            {
                AccountId = accountId,
                Value = account.Cookie.CookieString,
                CreateDateTime = account.Cookie.CreateDateTime
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


