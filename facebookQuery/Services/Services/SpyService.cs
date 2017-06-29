using System;
using System.Collections.Generic;
using System.Linq;
using CommonInterfaces.Interfaces.Services;
using CommonInterfaces.Interfaces.Services.BackgroundJobs;
using CommonInterfaces.Interfaces.Services.Jobs;
using Constants.GendersUnums;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.Cookies;
using DataBase.QueriesAndCommands.Commands.SpyAccounts;
using DataBase.QueriesAndCommands.Queries.Account.Models;
using DataBase.QueriesAndCommands.Queries.Account.SpyAccount;
using DataBase.QueriesAndCommands.Queries.Friends;
using Engines.Engines.GetFriendsEngine.CheckFriendInfoBySeleniumEngine;
using Engines.Engines.GetNewCookiesEngine;
using Hangfire;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;
using Services.Core;
using Services.Core.Models;
using Services.Interfaces;
using Services.Interfaces.ServiceTools;
using Services.Models.Jobs;
using Services.ServiceTools;
using Services.ViewModels.AccountModels;
using Services.ViewModels.GroupModels;
using Services.ViewModels.HomeModels;
using Services.ViewModels.SpyAccountModels;

namespace Services.Services
{
    public class SpyService
    {
        private readonly ISpyAccountManager _spyAccountManager;
        private readonly IProxyManager _proxyManager;
        private readonly IAccountManager _accountManager;
        private readonly IAccountSettingsManager _accountSettingsManager;
        private readonly ISeleniumManager _seleniumManager;
        private readonly IJobService _jobService;

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
                Name = model.Name,
                ProxyDataIsFailed = model.ProxyDataIsFailed,
                ConformationIsFailed = model.ConformationIsFailed,
                AuthorizationDataIsFailed = model.AuthorizationDataIsFailed
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

            var model = new RemoveAccountJobsModel
            {
                Login = spyAccount.Login,
                AccountId = null
            };

            _jobService.RemoveAccountJobs(model);
        }

        public void AddOrUpdateSpyAccount(SpyAccountViewModel model, IBackgroundJobService backgroundJobService)
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

            new CookieService().RefreshCookies(new AccountViewModel
            {
                Id = accountId,
                Login = model.Login,
                Password = model.Password,
                Proxy = model.Proxy,
                ProxyLogin = model.ProxyLogin,
                ProxyPassword = model.ProxyPassword,
            }, 
            true,
            backgroundJobService);
            
            var account = new GetSpyAccountByIdQueryHandler(new DataBaseContext()).Handle(new GetSpyAccountByIdQuery
            {
                UserId = accountId
            });

            if (model.PageUrl == null || model.FacebookId == 0)
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

            var jobModel = new AddOrUpdateAccountModel
            {
                Account = new AccountViewModel
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
                }
            };

            _jobService.AddOrUpdateSpyAccountJobs(jobModel);
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

            var driver = _seleniumManager.RegisterNewDriver(accountViewModel); // открываем драйвер для spy
            try
            {
                var friendList = new GetAnalisysFriendsQueryHandler(new DataBaseContext()).Handle(new GetAnalisysFriendsQuery());

                foreach (var analysisFriendData in friendList)
                {
                    var accountAnalysisFriend = _accountManager.GetAccountById(analysisFriendData.AccountId);

                    if (!new AccountManager().HasAWorkingAccount(accountAnalysisFriend.Id))
                    {
                        continue;
                    }

                    var settingsModel = accountAnalysisFriend.GroupSettingsId != null
                        ? _accountSettingsManager.GetSettings((long)accountAnalysisFriend.GroupSettingsId)
                        : new GroupSettingsViewModel();

                    var settings = new GroupSettingsViewModel
                    {
                        GroupId = settingsModel.GroupId,
                        Gender = settingsModel.Gender,
                        Countries = settingsModel.Countries,
                        Cities = settingsModel.Cities
                    };

                    var friendIsSuccess = AnalizeFriend(new AccountViewModel
                    {
                        FacebookId = spyAccount.FacebookId,
                        Cookie = spyAccount.Cookie.CookieString,
                        Proxy = spyAccount.Proxy,
                        ProxyLogin = spyAccount.ProxyLogin,
                        ProxyPassword = spyAccount.ProxyPassword
                    }, analysisFriendData.FacebookId,
                    settings,
                    driver);

                    new AnalizeFriendCore().StartAnalyze(new AnalyzeModel
                    {
                        Settings = settings,
                        AnalysisFriend = analysisFriendData,
                        InfoIsSuccess = friendIsSuccess,
                        SpyAccountId = spyAccount.Id
                    });
                }

            }
            catch (Exception)
            {
                driver.Quit();
            }

            driver.Quit();
        }

        private bool AnalizeFriendInfo(AccountViewModel account, long friendFacebookId, GroupSettingsViewModel settings, RemoteWebDriver driver)
        {
            var infoIsSuccess = new CheckFriendInfoBySeleniumEngine().Execute(new CheckFriendInfoBySeleniumModel
            {
                AccountFacebookId = account.FacebookId,
                FriendFacebookId = friendFacebookId,
                Cities = settings.Cities,
                Countries = settings.Countries,
                Cookie = account.Cookie,
                Driver = driver
            });

            return infoIsSuccess;
        }

        private bool AnalizeFriendGender(AccountViewModel account, long friendFacebookId, GroupSettingsViewModel settings)
        {
            if (settings.Gender != null)
            {
                var genderIsSuccess = new CheckFriendGenderEngine().Execute(new CheckFriendGenderModel
                {
                    AccountFacebookId = account.FacebookId,
                    FriendFacebookId = friendFacebookId,
                    Gender = (GenderEnum)settings.Gender,
                    Cookie = account.Cookie,
                    Proxy = _accountManager.GetAccountProxy(account)
                });

                return genderIsSuccess;
            }
            return false;
        }

        public bool AnalizeFriend(AccountViewModel account, long friendFacebookId, GroupSettingsViewModel settings, RemoteWebDriver driver)
        {
            bool infoIsSuccess;
            bool genderIsSuccess;
            if (settings == null)
            {
                return false;
            }

            if ((settings.Countries != null && !settings.Countries.Equals(string.Empty)) || (settings.Cities != null && !settings.Cities.Equals(string.Empty)))
            {
                infoIsSuccess = AnalizeFriendInfo(account, friendFacebookId, settings, driver);
            }
            else
            {
                infoIsSuccess = true;
            }

            if (settings.Gender != null && infoIsSuccess) //Если нужно проверить пол и при этом ПОДОШЁЛ по данным локации
            {
                genderIsSuccess = AnalizeFriendGender(account, friendFacebookId, settings);
            }
            else
            {
                genderIsSuccess = true;
            }

            return infoIsSuccess && genderIsSuccess;
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
