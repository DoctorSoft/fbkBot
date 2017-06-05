using System.Collections.Generic;
using System.Linq;
using CommonInterfaces.Interfaces.Services;
using DataBase.Context;
using DataBase.Migrations;
using DataBase.QueriesAndCommands.Commands.Accounts;
using DataBase.QueriesAndCommands.Commands.Cookies;
using DataBase.QueriesAndCommands.Queries.Account;
using DataBase.QueriesAndCommands.Queries.Account.GetWorkAccounts;
using DataBase.QueriesAndCommands.Queries.Account.Models;
using DataBase.QueriesAndCommands.Queries.Groups.GroupSettings;
using DataBase.QueriesAndCommands.Queries.UserAgent.GetRandomUserAgent;
using DataBase.QueriesAndCommands.Queries.UserAgent.GetUserAgentById;
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
        private readonly JobStatusManager _jobStatusManager;
        private readonly IAccountSettingsManager _accountSettingsManager;
        private readonly IStatisticsManager _accountStatisticsManager;
        private readonly IProxyManager _proxyManager;
        private readonly IJobService _jobService;
        private readonly IBackgroundJobService _backgroundJobService;

        public HomeService(IJobService jobService, IBackgroundJobService backgroundJobService)
        {
            _accountManager = new AccountManager();
            _jobStatusManager = new JobStatusManager();
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

            return accounts.Select(accountModel => new AccountViewModel
            {
                Id = accountModel.Id,
                PageUrl = accountModel.PageUrl,
                Login = accountModel.Login,
                Password = accountModel.Password,
                FacebookId = accountModel.FacebookId,
                Proxy = accountModel.Proxy,
                ProxyLogin = accountModel.ProxyLogin,
                ProxyPassword = accountModel.ProxyPassword,
                Cookie = accountModel.Cookie.CookieString,
                Name = accountModel.Name,
                GroupSettingsId = accountModel.GroupSettingsId,
                AuthorizationDataIsFailed = accountModel.AuthorizationDataIsFailed,
                ProxyDataIsFailed = accountModel.ProxyDataIsFailed,
                IsDeleted = accountModel.IsDeleted,
                ConformationDataIsFailed = accountModel.ConformationIsFailed,
                UserAgentId = accountModel.UserAgentId
            }).ToList();
        }

        public List<AccountViewModel> GetAccountsWithErrors()
        {
            var accounts = new GetAccountsQueryHandler(new DataBaseContext()).Handle(new GetAccountsQuery
            {
                Count = 100,
                Page = 0
            });

            return accounts.Where(model => model.AuthorizationDataIsFailed || model.ConformationIsFailed || model.ProxyDataIsFailed)
                .Select(accountModel => new AccountViewModel
                {
                    Id = accountModel.Id,
                    PageUrl = accountModel.PageUrl,
                    Login = accountModel.Login,
                    Password = accountModel.Password,
                    FacebookId = accountModel.FacebookId,
                    Proxy = accountModel.Proxy,
                    ProxyLogin = accountModel.ProxyLogin,
                    ProxyPassword = accountModel.ProxyPassword,
                    Cookie = accountModel.Cookie.CookieString,
                    Name = accountModel.Name,
                    GroupSettingsId = accountModel.GroupSettingsId,
                    AuthorizationDataIsFailed = accountModel.AuthorizationDataIsFailed,
                    ProxyDataIsFailed = accountModel.ProxyDataIsFailed,
                    IsDeleted = accountModel.IsDeleted,
                    ConformationDataIsFailed = accountModel.ConformationIsFailed,
                    UserAgentId = accountModel.UserAgentId
                }).ToList();
        }

        public List<AccountDataViewModel> GetDataAccounts(List<AccountViewModel> accounts)
        {
            var forSpy = false;
            var result = new List<AccountDataViewModel>();

            foreach (var accountModel in accounts)
            {
                if (accountModel.GroupSettingsId != null)
                {
                    var information = _accountManager.GetAccountInformation(accountModel.Id);
                    var jobStatuses = _jobStatusManager.GetAllJobStatusesByAccountId(accountModel.Id, forSpy);

                    result.Add(new AccountDataViewModel
                    {
                        Account = new AccountViewModel
                        {
                            Id = accountModel.Id,
                            PageUrl = accountModel.PageUrl,
                            Login = accountModel.Login,
                            Password = accountModel.Password,
                            FacebookId = accountModel.FacebookId,
                            Proxy = accountModel.Proxy,
                            ProxyLogin = accountModel.ProxyLogin,
                            ProxyPassword = accountModel.ProxyPassword,
                            Cookie = accountModel.Cookie,
                            Name = accountModel.Name,
                            GroupSettingsId = accountModel.GroupSettingsId,
                            AuthorizationDataIsFailed = accountModel.AuthorizationDataIsFailed,
                            ProxyDataIsFailed = accountModel.ProxyDataIsFailed,
                            IsDeleted = accountModel.IsDeleted,
                            ConformationDataIsFailed = accountModel.ConformationDataIsFailed,
                            UserAgentId = accountModel.UserAgentId,
                            GroupSettingsName = new GetGroupSettingsNameByIdQueryHandler(new DataBaseContext()).Handle(new GetGroupSettingsNameByIdQuery { GroupId = accountModel.GroupSettingsId})
                        },
                        AccountInformation = information,
                        JobStatuses = jobStatuses
                    });

                    continue;
                }
                result.Add(new AccountDataViewModel
                {
                    Account = new AccountViewModel
                    {
                        Id = accountModel.Id,
                        PageUrl = accountModel.PageUrl,
                        Login = accountModel.Login,
                        Password = accountModel.Password,
                        FacebookId = accountModel.FacebookId,
                        Proxy = accountModel.Proxy,
                        ProxyLogin = accountModel.ProxyLogin,
                        ProxyPassword = accountModel.ProxyPassword,
                        Cookie = accountModel.Cookie,
                        Name = accountModel.Name,
                        GroupSettingsId = accountModel.GroupSettingsId,
                        AuthorizationDataIsFailed = accountModel.AuthorizationDataIsFailed,
                        ProxyDataIsFailed = accountModel.ProxyDataIsFailed,
                        IsDeleted = accountModel.IsDeleted,
                        ConformationDataIsFailed = accountModel.ConformationDataIsFailed,
                        UserAgentId = accountModel.UserAgentId,
                        GroupSettingsName = new GetGroupSettingsNameByIdQueryHandler(new DataBaseContext()).Handle(new GetGroupSettingsNameByIdQuery { GroupId = accountModel.GroupSettingsId })
                        
                    }
                });
            }

            var orderAccounts = _accountManager.SortAccountsByWorkStatus(result);

            return orderAccounts;
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
                IsDeleted = model.IsDeleted,
                UserAgentId = model.UserAgentId,
                GroupSettingsName = new GetGroupSettingsNameByIdQueryHandler(new DataBaseContext()).Handle(new GetGroupSettingsNameByIdQuery { GroupId = model.GroupSettingsId })
                        
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
                IsDeleted = model.IsDeleted,
                UserAgentId = model.UserAgentId
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
                UserAgentId = account.UserAgentId
            }, 
            forSpy: false, 
            backgroundJob: backgroundJobService);

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
                ConformationDataIsFailed = account.ConformationIsFailed,
                UserAgentId = account.UserAgentId
            };

            var model = new AddOrUpdateAccountModel()
            {
                Account = accountViewModel,
                NewSettings = settings,
                OldSettings = null
            };

            backgroundJobService.AddOrUpdateAccountJobs(model);
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
                    Cookie = account.Cookie,
                    GroupSettingsId = account.GroupSettingsId,
                    AuthorizationDataIsFailed = account.AuthorizationDataIsFailed,
                    IsDeleted = account.IsDeleted,
                    ProxyDataIsFailed = account.ProxyDataIsFailed,
                    ConformationDataIsFailed = account.ConformationDataIsFailed,
                    UserAgentId = account.UserAgentId
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

        public long AddOrUpdateAccount(AccountDraftViewModel model, IBackgroundJobService backgroundJobService)
        {
            var userAgentId = new GetRandomUserAgentQueryHandler(new DataBaseContext()).Handle(new GetRandomUserAgentQuery());
            if (userAgentId == null)
            {
                return 0;
            }

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
                ProxyPassword = model.ProxyPassword,
                UserAgentId = userAgentId.Id
            });

            new CookieService().RefreshCookies(new AccountViewModel
            {
                Id = accountId,
                Login = model.Login,
                Password = model.Password,
                Proxy = model.Proxy,
                ProxyLogin = model.ProxyLogin,
                ProxyPassword = model.ProxyPassword,
                UserAgentId = model.UserAgentId
            }, 
            false,
            backgroundJobService);


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

            var modelForJob = new AddOrUpdateAccountModel
            {
                Account = new AccountViewModel
                {
                    Id = accountId,
                    PageUrl = account.PageUrl,
                    Login = account.Login,
                    Password = account.Password,
                    FacebookId = account.FacebookId,
                    Proxy = account.Proxy,
                    ProxyLogin = account.ProxyLogin,
                    ProxyPassword = account.ProxyPassword,
                    Cookie = account.Cookie.CookieString,
                    Name = model.Name,
                    GroupSettingsId = account.GroupSettingsId,
                    AuthorizationDataIsFailed = account.AuthorizationDataIsFailed,
                    ProxyDataIsFailed = account.ProxyDataIsFailed,
                    ConformationDataIsFailed = account.ConformationIsFailed,
                    IsDeleted = account.IsDeleted,
                    UserAgentId = account.UserAgentId
                }
            };

            _jobService.AddOrUpdateAccountJobs(modelForJob);
            
            return accountId;
        }

        public CookiesViewModel GetAccountCookies(long accountId)
        {
            var account = new GetAccountByIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByIdQuery
            {
                UserId = accountId
            });

            var userAgent = new GetUserAgentQueryHandler(new DataBaseContext()).Handle(new GetUserAgentQuery
            {
                UserAgentId = account.UserAgentId
            });

            if (account.Cookie == null)
            {
                return new CookiesViewModel
                {
                    AccountId = accountId,
                    Value = "Cookie is not created",
                    UserAgent = userAgent.UserAgentString
                };
            }
            return new CookiesViewModel
            {
                AccountId = accountId,
                Value = account.Cookie.CookieString,
                CreateDateTime = account.Cookie.CreateDateTime,
                UserAgent = userAgent.UserAgentString
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


