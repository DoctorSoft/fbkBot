using System;
using System.Collections.Generic;
using System.Linq;
using CommonModels;
using Constants;
using Constants.EnumExtension;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.Accounts;
using DataBase.QueriesAndCommands.Commands.Cookies;
using DataBase.QueriesAndCommands.Queries.Account;
using DataBase.QueriesAndCommands.Queries.Account.Models;
using Engines.Engines.GetNewCookiesEngine;
using Engines.Engines.GetNewNoticesEngine;
using OpenQA.Selenium.PhantomJS;
using RequestsHelpers;
using Services.Core.Interfaces.ServiceTools;
using Services.Interfaces;
using Services.ServiceTools;
using Services.ViewModels.AccountModels;
using Services.ViewModels.GroupModels;
using Services.ViewModels.HomeModels;

namespace Services.Services
{
    public class HomeService
    {
        private IAccountManager _accountManager;
        private IAccountSettingsManager _accountSettingsManager;
        private IStatisticsManager _accountStatisticsManager;
        private IProxyManager _proxyManager;
        private IJobService _jobService;

        public HomeService(IJobService jobService, IAccountManager accountManager, IAccountSettingsManager accountSettingsManager)
        {
            _jobService = jobService;
            _accountManager = accountManager;
            _accountSettingsManager = accountSettingsManager;
            _accountStatisticsManager = new StatisticsManager();
            _proxyManager = new ProxyManager();
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
                FacebookId = model.FacebookId,
                Proxy = model.Proxy,
                ProxyLogin = model.ProxyLogin,
                ProxyPassword = model.ProxyPassword,
                Cookie = model.Cookie.CookieString,
                Name = model.Name,
                GroupSettingsId = model.GroupSettingsId
            }).ToList();
        }

        public List<AccountViewModel> GetDeletedAccounts()
        {
            var accounts = new GetDeletedAccountsQueryHandler(new DataBaseContext()).Handle(new GetDeletedAccountsQuery
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
                FacebookId = model.FacebookId,
                Proxy = model.Proxy,
                ProxyLogin = model.ProxyLogin,
                ProxyPassword = model.ProxyPassword,
                Cookie = model.Cookie.CookieString,
                Name = model.Name
            }).ToList();
        } 

        public void RemoveAccount(long accountId)
        {
            var account = new GetAccountByIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByIdQuery
            {
                UserId = accountId,
                SearchDeleted = true
            });

            new DeleteUserCommandHandler(new DataBaseContext()).Handle(new DeleteUserCommand
            {
                AccountId = accountId
            });

            _jobService.RemoveAccountJobs(account.Login);
        }

        public void RecoverAccount(long accountId)
        {
            new RecoverUserCommandHandler(new DataBaseContext()).Handle(new RecoverUserCommand
            {
                AccountId = accountId
            });

            var account = new GetAccountByIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByIdQuery
            {
                UserId = accountId
            });

            RefreshCookies(new AccountViewModel
            {
                Id = accountId,
                Login = account.Login,
                Password = account.Password,
                Proxy = account.Proxy,
                ProxyLogin = account.ProxyLogin,
                ProxyPassword = account.ProxyPassword,
            });

            account = new GetAccountByIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByIdQuery
            {
                UserId = accountId
            });

            _jobService.AddOrUpdateAccountJobs(new AccountViewModel
            {
                Id = accountId,
                Name = account.Name,
                PageUrl = account.PageUrl,
                FacebookId = account.FacebookId,
                Password = account.Password,
                Login = account.Login,
                Proxy = account.Proxy,
                ProxyLogin = account.ProxyLogin,
                ProxyPassword = account.ProxyPassword,
                Cookie = account.Cookie.CookieString
            });
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
                FacebookUserId = accountId
            });
            var statusModel = new GetNewNoticesEngine().Execute(new GetNewNoticesModel()
            {
                ResponsePage = RequestsHelper.Get(Urls.HomePage.GetDiscription(), account.Cookie.CookieString, _accountManager.GetAccountProxy(account)) 
            });

            return statusModel;
        }

        public AccountModel GetAccountByUserId(long? userId)
        {
            if (userId == null)
            {
                return new AccountModel();
            }

            return new GetAccountByFacebookIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByFacebookIdQuery
            {
                FacebookUserId = userId.Value
            });
        }

        public AccountSettingsViewModel GetAccountSettings(long accountId)
        {
            var account =  _accountManager.GetAccountById(accountId);
            var settings = new SettingsModel();
            if (account.GroupSettingsId != null)
            {
                settings = _accountSettingsManager.GetSettings((long)account.GroupSettingsId);
            }
            
            var statistics = _accountStatisticsManager.GetAccountStatistics(accountId);

            var detailedStatistic = new DetailedStatisticsModel()
            {
                AllTimeStatistic = _accountStatisticsManager.GetLastHourAccountStatistics(statistics),
                LastHourStatistic = _accountStatisticsManager.GetAllTimeAccountStatistics(statistics)
            };
            
            var accountViewModel = new AccountSettingsViewModel
            {
                Settings = settings,
                Statistics = detailedStatistic,
                Account = new AccountViewModel
                {
                    Id = accountId,
                    Name = account.Name,
                    PageUrl = account.PageUrl,
                    FacebookId = account.FacebookId,
                    Password = account.Password,
                    Login = account.Login,
                    Proxy = account.Proxy,
                    ProxyLogin = account.ProxyLogin,
                    ProxyPassword = account.ProxyPassword,
                    Cookie = account.Cookie.CookieString
                }
            };

            return accountViewModel;
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
                Id = accountId,
                Login = model.Login,
                Password = model.Password,
                Proxy = model.Proxy,
                ProxyLogin = model.ProxyLogin,
                ProxyPassword = model.ProxyPassword,
            });


            var account = new GetAccountByIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByIdQuery
            {
                UserId = accountId
            });

            if (model.Id == null || model.PageUrl == null || model.FacebookId == 0)
            {
                var accountFacebookId = _proxyManager.GetAccountFacebookId(account.Cookie.CookieString);
                var homePageUrl = _accountManager.CreateHomePageUrl(accountFacebookId);

                new AddOrUpdateAccountCommandHandler(new DataBaseContext()).Handle(new AddOrUpdateAccountCommand
                {
                    Id = account.Id,
                    Name = model.Name,
                    PageUrl = homePageUrl,
                    FacebookId = accountFacebookId,
                    Password = model.Password,
                    Login = model.Login,
                    Proxy = model.Proxy,
                    ProxyLogin = model.ProxyLogin,
                    ProxyPassword = model.ProxyPassword
                });

                account = new GetAccountByIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByIdQuery
                {
                    UserId = accountId
                });
            }

            _jobService.AddOrUpdateAccountJobs(new AccountViewModel
            {
                Id = accountId,
                Name = model.Name,
                PageUrl = model.PageUrl,
                FacebookId = model.FacebookId,
                Password = model.Password,
                Login = model.Login,
                Proxy = model.Proxy,
                ProxyLogin = model.ProxyLogin,
                ProxyPassword = model.ProxyPassword,
                Cookie = account.Cookie.CookieString
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

        public void UpdateSettings(SettingsModel newOptions)
        {
            _accountSettingsManager.UpdateSettings(newOptions);
        }
    }
}


