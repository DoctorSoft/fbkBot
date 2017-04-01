using System.Collections.Generic;
using System.Linq;
using CommonInterfaces.Interfaces.Services;
using Constants;
using Constants.EnumExtension;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.Accounts;
using DataBase.QueriesAndCommands.Commands.Cookies;
using DataBase.QueriesAndCommands.Queries.Account;
using DataBase.QueriesAndCommands.Queries.Account.GetWorkAccounts;
using DataBase.QueriesAndCommands.Queries.Account.Models;
using Engines.Engines.GetNewNoticesEngine;
using RequestsHelpers;
using Services.Interfaces.ServiceTools;
using Services.Models.Jobs;
using Services.ServiceTools;
using Services.ViewModels.AccountModels;
using Services.ViewModels.GroupModels;
using Services.ViewModels.HomeModels;
using AddOrUpdateAccountModel = Services.Models.BackgroundJobs.AddOrUpdateAccountModel;

namespace Services.Services
{
    public class HomeService
    {
        private readonly IAccountManager _accountManager;
        private readonly IAccountSettingsManager _accountSettingsManager;
        private readonly IStatisticsManager _accountStatisticsManager;
        private readonly IProxyManager _proxyManager;
        private readonly IJobService _jobService;
        private readonly IBackgroundJobService _backgroundJobService;

        public HomeService(IJobService jobService, IBackgroundJobService backgroundJobService)
        {
            _accountManager = new AccountManager();
            _accountSettingsManager = new AccountSettingsManager();
            _accountStatisticsManager = new StatisticsManager();
            _proxyManager = new ProxyManager();
            _jobService = jobService;
            _backgroundJobService = backgroundJobService;
        }

        public List<AccountViewModel> GetAccounts()
        {
            var accounts = new GetAccountsQueryHandler(new DataBaseContext()).Handle(new GetAccountsQuery
            {
                Count = 100,
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
                GroupSettingsId = model.GroupSettingsId,
                AuthorizationDataIsFailed = model.AuthorizationDataIsFailed,
                ProxyDataIsFailed = model.ProxyDataIsFailed,
                IsDeleted = model.IsDeleted,
                ConformationDataIsFailed = model.ConformationIsFailed
            }).ToList();
        }


        public List<AccountViewModel> GetWorkAccounts()
        {
            var accounts = new GetWorkAccountsQueryHandler(new DataBaseContext()).Handle(new GetWorkAccountsQuery());

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
                GroupSettingsId = model.GroupSettingsId,
                AuthorizationDataIsFailed = model.AuthorizationDataIsFailed,
                ProxyDataIsFailed = model.ProxyDataIsFailed,
                ConformationDataIsFailed = model.ConformationIsFailed,
                IsDeleted = model.IsDeleted
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
                Name = model.Name,
                GroupSettingsId = model.GroupSettingsId,
                AuthorizationDataIsFailed = model.AuthorizationDataIsFailed,
                ProxyDataIsFailed = model.ProxyDataIsFailed,
                ConformationDataIsFailed = model.ConformationIsFailed,
                IsDeleted = model.IsDeleted
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


            var jobModel = new RemoveAccountJobsModel()
            {
                AccountId = account.Id,
                Login = account.Login
            };

            _jobService.RemoveAccountJobs(jobModel);
            _backgroundJobService.RemoveAccountBackgroundJobs(jobModel);
        }

        public void RecoverAccount(long accountId, IBackgroundJobService backgroundJobService)
        {
            new RecoverUserCommandHandler(new DataBaseContext()).Handle(new RecoverUserCommand
            {
                AccountId = accountId
            });

            var account = new GetAccountByIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByIdQuery
            {
                UserId = accountId
            });

            new CookieService().RefreshCookies(new AccountViewModel
            {
                Id = accountId,
                Login = account.Login,
                Password = account.Password,
                Proxy = account.Proxy,
                ProxyLogin = account.ProxyLogin,
                ProxyPassword = account.ProxyPassword,
            });

            if (account.GroupSettingsId == null)
            {
                return;
            }

            var settings = _accountSettingsManager.GetSettings((long)account.GroupSettingsId);
            var accountViewModel = new AccountViewModel
            {
                Id = account.Id,
                AuthorizationDataIsFailed = account.AuthorizationDataIsFailed,
                Cookie = account.Cookie.CookieString,
                IsDeleted = account.IsDeleted,
                ProxyDataIsFailed = account.ProxyDataIsFailed,
                GroupSettingsId = account.GroupSettingsId,
                Name = account.Name,
                Proxy = account.Proxy,
                FacebookId = account.FacebookId,
                Login = account.Login,
                PageUrl = account.PageUrl,
                Password = account.Password,
                ProxyLogin = account.ProxyLogin,
                ProxyPassword = account.ProxyPassword,
                ConformationDataIsFailed = account.ConformationIsFailed
            };

            var model = new AddOrUpdateAccountModel()
            {
                Account = accountViewModel,
                NewSettings = settings,
                OldSettings = null
            };

            backgroundJobService.AddOrUpdateAccountJobs(model);
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
            var statistics = _accountStatisticsManager.GetAccountStatistics(accountId);

            var detailedStatistic = new DetailedStatisticsModel
            {
                AllTimeStatistic = _accountStatisticsManager.GetAllTimeAccountStatistics(statistics),
                LastHourStatistic = _accountStatisticsManager.GetLastHourAccountStatistics(statistics),
            };

            var accountViewModel = new AccountSettingsViewModel
            {
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
                    Cookie = account.Cookie.CookieString,
                    GroupSettingsId = account.GroupSettingsId,
                    AuthorizationDataIsFailed = account.AuthorizationDataIsFailed,
                    IsDeleted = account.IsDeleted,
                    ProxyDataIsFailed = account.ProxyDataIsFailed,
                    ConformationDataIsFailed = account.ConformationIsFailed
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

            new CookieService().RefreshCookies(new AccountViewModel
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

                return accountId;
            }

            //backgroundJobService.AddOrUpdateAccountJobs(new AccountViewModel(), ) 
            
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

        public void UpdateSettings(GroupSettingsViewModel newOptions)
        {
            _accountSettingsManager.UpdateSettings(newOptions);
        }

    }
}


