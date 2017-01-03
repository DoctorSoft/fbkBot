﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.Cookies;
using DataBase.QueriesAndCommands.Commands.SpyAccounts;
using DataBase.QueriesAndCommands.Queries.Account;
using DataBase.QueriesAndCommands.Queries.Account.Models;
using DataBase.QueriesAndCommands.Queries.Account.SpyAccount;
using DataBase.QueriesAndCommands.Queries.Friends;
using Engines.Engines.GetFriendInfoEngine;
using Engines.Engines.GetNewCookiesEngine;
using OpenQA.Selenium.PhantomJS;
using Services.Core;
using Services.Core.Interfaces.ServiceTools;
using Services.Interfaces;
using Services.ServiceTools;
using Services.ViewModels.FriendsModels;
using Services.ViewModels.HomeModels;
using Services.ViewModels.SpyAccountModels;
using AccountSettingsModel = Engines.Engines.GetFriendInfoEngine.AccountSettingsModel;

namespace Services.Services
{
    public class SpyService
    {
        private readonly ISpyAccountManager _spyAccountManager;
        private readonly IAccountManager _accountManager;
        private readonly IAccountSettingsManager _accountSettingsManager;
        private IJobService _jobService;

        public SpyService(IJobService jobService)
        {
            _jobService = jobService;
            _spyAccountManager = new SpyAccountManager();
            _accountManager = new AccountManager();
            _accountSettingsManager = new AccountSettingsManager();
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

            var account = new GetAccountByIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByIdQuery
            {
                UserId = accountId
            });

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
                Cookie = spyAccount.Cookie.CookieString,
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
            var account = new AccountModel()
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
                UserId = accountViewModel.Id,
                Cookie = new CookieModel
                {
                    CookieString = accountViewModel.Cookie
                }
            };

            var friendList = new GetAnalisysFriendsQueryHandler(new DataBaseContext()).Handle(new GetAnalisysFriendsQuery());
            var settings = _accountSettingsManager.GetAccountSettings(accountViewModel.Id);

            foreach (var analysisFriendData in friendList)
            {
                var friendInfo = new GetFriendInfoEngine().Execute(new GetFriendInfoModel()
                {
                    AccountFacebookId = account.FacebookId,
                    Proxy = _accountManager.GetAccountProxy(account),
                    Cookie = account.Cookie.CookieString,
                    FriendFacebookId = analysisFriendData.FacebookId,
                    Settings = new AccountSettingsModel
                    {
                        AccountId = settings.AccountId,
                        Gender = settings.Gender,
                        LivesPlace = settings.LivesPlace,
                        SchoolPlace = settings.SchoolPlace,
                        WorkPlace = settings.WorkPlace
                    }
                });


                new AnalizeFriendCore().StartAnalyze(settings, new FriendInfoViewModel
                {
                    Id = analysisFriendData.Id,
                    FacebookId = analysisFriendData.FacebookId,
                    Gender = friendInfo.Gender,
                    LivesSection = friendInfo.LivesSection,
                    RelationsSection = friendInfo.RelationsSection,
                    SchoolSection = friendInfo.SchoolSection,
                    WorkSection = friendInfo.WorkSection
                });

                Thread.Sleep(5000);
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
    }
}
