using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Constants.FriendTypesEnum;
using Constants.GendersUnums;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.Accounts;
using DataBase.QueriesAndCommands.Commands.Cookies;
using DataBase.QueriesAndCommands.Commands.Friends.ChangeAnalysisFriendStatusCommand;
using DataBase.QueriesAndCommands.Commands.Friends.RemoveAnalyzedFriendCommand;
using DataBase.QueriesAndCommands.Commands.SpyAccounts;
using DataBase.QueriesAndCommands.Commands.SpyStatistics;
using DataBase.QueriesAndCommands.Queries.Account;
using DataBase.QueriesAndCommands.Queries.Account.Models;
using DataBase.QueriesAndCommands.Queries.Account.SpyAccount;
using DataBase.QueriesAndCommands.Queries.Friends;
using Engines.Engines.GetFriendInfoEngine;
using Engines.Engines.GetFriendsEngine.CheckFriendInfoBySeleniumEngine;
using Engines.Engines.GetNewCookiesEngine;
using OpenQA.Selenium.PhantomJS;
using Services.Core;
using Services.Core.Models;
using Services.Interfaces;
using Services.Interfaces.ServiceTools;
using Services.ServiceTools;
using Services.ViewModels.AccountModels;
using Services.ViewModels.FriendsModels;
using Services.ViewModels.GroupModels;
using Services.ViewModels.HomeModels;
using Services.ViewModels.SpyAccountModels;
using CommonModels;

namespace Services.Services
{
    public class SpyService
    {
        private readonly ISpyAccountManager _spyAccountManager;
        private readonly IProxyManager _proxyManager;
        private readonly IAccountManager _accountManager;
        private readonly IAccountSettingsManager _accountSettingsManager;
        private readonly ISeleniumManager _seleniumManager;
        private IJobService _jobService;

        public SpyService(IJobService jobService)
        {
            _jobService = jobService;
            _spyAccountManager = new SpyAccountManager();
            _accountManager = new AccountManager();
            _accountSettingsManager = new AccountSettingsManager();
            _proxyManager = new ProxyManager();
            _seleniumManager = new SeleniumManager();
        }

        public List<SpyAccountViewModel> GetSpyAccounts()
        {
            var accounts = new GetSpyAccountsQueryHandler(new DataBaseContext()).Handle(new GetSpyAccountsQuery
            {
                Count = 10,
                Page = 0
            });

            return accounts.Select(model => new SpyAccountViewModel
            {
                Id = model.Id,
                PageUrl = model.PageUrl,
                Login = model.Login,
                Password = model.Password,
                FacebookId = model.UserId,
                Proxy = model.Proxy,
                ProxyLogin = model.ProxyLogin,
                ProxyPassword = model.ProxyPassword,
                Cookie = model.Cookie.CookieString,
                Name = model.Name
            }).ToList();
        }

        public void RemoveSpyAccount(long spyAccountId)
        {
            var spyAccount = new GetSpyAccountByIdQueryHandler(new DataBaseContext()).Handle(new GetSpyAccountByIdQuery
            {
                UserId = spyAccountId
            });

            new DeleteSpyAccounCommandHandler(new DataBaseContext()).Handle(new DeleteSpyAccounCommand
            {
                AccountId = spyAccountId
            });

            _jobService.RemoveAccountJobs(spyAccount.Login, null);
        }

        public void AddOrUpdateSpyAccount(SpyAccountViewModel model)
        {
            var accountId = new AddOrUpdateSpyAccountCommandHandler(new DataBaseContext()).Handle(new AddOrUpdateSpyAccountCommand
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

            RefreshCookies(new SpyAccountViewModel
            {
                Id = accountId,
                Login = model.Login,
                Password = model.Password,
                Proxy = model.Proxy,
                ProxyLogin = model.ProxyLogin,
                ProxyPassword = model.ProxyPassword,
            });

            var account = new GetSpyAccountByIdQueryHandler(new DataBaseContext()).Handle(new GetSpyAccountByIdQuery
            {
                UserId = accountId
            });
            if (model.Id == null || model.PageUrl == null || model.FacebookId == 0)
            {
                var accountFacebookId = _proxyManager.GetAccountFacebookId(account.Cookie.CookieString);
                var homePageUrl = _accountManager.CreateHomePageUrl(accountFacebookId);

                new AddOrUpdateSpyAccountCommandHandler(new DataBaseContext()).Handle(new AddOrUpdateSpyAccountCommand
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

                account = new GetSpyAccountByIdQueryHandler(new DataBaseContext()).Handle(new GetSpyAccountByIdQuery
                {
                    UserId = accountId
                });
            }
            _jobService.AddOrUpdateSpyAccountJobs(new AccountViewModel
            {
                Id = accountId,
                Name = model.Name,
                PageUrl = model.PageUrl,
                FacebookId = account.FacebookId,
                Password = model.Password,
                Login = model.Login,
                Proxy = model.Proxy,
                ProxyLogin = model.ProxyLogin,
                ProxyPassword = model.ProxyPassword,
                Cookie = account.Cookie.CookieString
            });
        }

        public SpyAccountViewModel GetSpyAccountById(long? spyAccountId)
        {
            if (spyAccountId == null)
            {
                return new SpyAccountViewModel();
            }
            var spyAccount = _spyAccountManager.GetSpyAccountById(spyAccountId);

            return new SpyAccountViewModel
            {
                Cookie = spyAccount.Cookie == null ? null : spyAccount.Cookie.CookieString,
                FacebookId = spyAccount.FacebookId,
                Id = spyAccount.Id,
                Login = spyAccount.Login,
                Name = spyAccount.Name,
                PageUrl = spyAccount.PageUrl,
                Proxy = spyAccount.Proxy,
                Password = spyAccount.Password,
                ProxyLogin = spyAccount.ProxyLogin,
                ProxyPassword = spyAccount.ProxyPassword
            };
        }

        public void AnalyzeFriends(AccountViewModel accountViewModel)
        {
            var spyAccount = new AccountModel
            {
                Id = accountViewModel.Id,
                FacebookId = accountViewModel.FacebookId,
                Login = accountViewModel.Login,
                Name = accountViewModel.Name,
                PageUrl = accountViewModel.PageUrl,
                Password = accountViewModel.Password,
                Proxy = accountViewModel.Proxy,
                ProxyLogin = accountViewModel.ProxyLogin,
                ProxyPassword = accountViewModel.ProxyPassword,
                Cookie = new CookieModel
                {
                    CookieString = accountViewModel.Cookie
                }
            };

            var friendList = new GetAnalisysFriendsQueryHandler(new DataBaseContext()).Handle(new GetAnalisysFriendsQuery());

            foreach (var analysisFriendData in friendList)
            {
                var accountAnalysisFriend = _accountManager.GetAccountById(analysisFriendData.AccountId);
                var settingsModel = accountAnalysisFriend.GroupSettingsId != null
                    ? _accountSettingsManager.GetSettings((long) accountAnalysisFriend.GroupSettingsId)
                    : new GroupSettingsViewModel();

                var settings = new GroupSettingsViewModel
                {
                    GroupId = settingsModel.GroupId,
                    Gender = settingsModel.Gender,
                    Countries = settingsModel.Countries,
                    Cities = settingsModel.Cities
                };

                var infoIsSuccess = false;
                var genderIsSuccess = false;

                if (settings.Countries != null || settings.Cities != null)
                {
                    infoIsSuccess = new CheckFriendInfoBySeleniumEngine().Execute(new CheckFriendInfoBySeleniumModel
                    {
                        AccountFacebookId = spyAccount.FacebookId,
                        FriendFacebookId = analysisFriendData.FacebookId,
                        Cities = settings.Cities,
                        Countries = settings.Countries,
                        Cookie = spyAccount.Cookie.CookieString,
                        Driver = _seleniumManager.RegisterNewDriver(new AccountViewModel
                        {
                            Proxy = spyAccount.Proxy,
                            ProxyLogin = spyAccount.ProxyLogin,
                            ProxyPassword = spyAccount.ProxyPassword
                        })
                    });
                }

                if (settings.Gender != null)
                {
                    genderIsSuccess = new CheckFriendGenderEngine().Execute(new CheckFriendGenderModel
                    {
                        AccountFacebookId = spyAccount.FacebookId,
                        FriendFacebookId = analysisFriendData.FacebookId,
                        Gender = (GenderEnum)settings.Gender,
                        Cookie = spyAccount.Cookie.CookieString,
                        Proxy = _accountManager.GetAccountProxy(spyAccount)
                    });
                }

                new AnalizeFriendCore().StartAnalyze(new AnalyzeModel
                {
                    Settings = settings,
                    AnalysisFriend = analysisFriendData,
                    GenderIsSuccess = genderIsSuccess,
                    InfoIsSuccess = infoIsSuccess,
                    SpyAccountId = spyAccount.Id
                });
            }
        }

        public bool RefreshCookies(SpyAccountViewModel account)
        {
            var driver = RegisterNewDriver(account);
            var newCookie = new GetNewCookiesEngine().Execute(new GetNewCookiesModel()
            {
                Login = account.Login,
                Password = account.Password,
                Driver = driver
            }).CookiesString;

            new UpdateCookiesForSpyHandler(new DataBaseContext()).Handle(new UpdateCookiesForSpyCommand
            {
                AccountId = account.Id,
                NewCookieString = newCookie
            });

            return true;
        }

        public PhantomJSDriver RegisterNewDriver(SpyAccountViewModel account)
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

        public CookiesViewModel GetSpyAccountCookies(long accountId)
        {
            var spyAccount = new GetSpyAccountByIdQueryHandler(new DataBaseContext()).Handle(new GetSpyAccountByIdQuery
            {
                UserId = accountId
            });

            if (spyAccount.Cookie == null)
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
                Value = spyAccount.Cookie.CookieString,
                CreateDateTime = spyAccount.Cookie.CreateDateTime
            };
        }

        public void UpdateCookies(CookiesViewModel model)
        {
            new UpdateCookiesForSpyHandler(new DataBaseContext()).Handle(new UpdateCookiesForSpyCommand
            {
                AccountId = model.AccountId,
                NewCookieString = model.Value
            });
        }
    }
}
