using CommonInterfaces.Interfaces.Services;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.Accounts;
using DataBase.QueriesAndCommands.Commands.Cookies;
using DataBase.QueriesAndCommands.Commands.SpyAccounts;
using Engines.Engines.CheckProxyEngine;
using Engines.Engines.GetNewCookiesEngine;
using Hangfire;
using Services.Interfaces.ServiceTools;
using Services.Models.Jobs;
using Services.ServiceTools;
using Services.ViewModels.HomeModels;

namespace Services.Services
{
    public class CookieService
    {
        private readonly ISeleniumManager _seleniumManager;

        public CookieService()
        {
            _seleniumManager = new SeleniumManager();
        }
        public bool RefreshCookies(AccountViewModel account, bool forSpy, IBackgroundJobService backgroundJob)
        {
            if (account.Proxy != null)
            {

                var proxyIsFailed = new CheckProxyEngine().Execute(new CheckProxyModel
                {
                    Driver = _seleniumManager.RegisterNewDriver(account)
                });

                if (proxyIsFailed)
                {
                    if (forSpy)
                    {
                        new UpdateFailSpyAccountInformationCommandHandler(new DataBaseContext()).Handle(
                        new UpdateFailSpyAccountInformationCommand
                        {
                            AccountId = account.Id,
                            ProxyDataIsFailed = true
                        });
                    }
                    else
                    {
                        new UpdateFailAccountInformationCommandHandler(new DataBaseContext()).Handle(
                        new UpdateFailAccountInformationCommand
                        {
                            AccountId = account.Id,
                            ProxyDataIsFailed = true
                        });
                    }
                    
                    backgroundJob.RemoveAccountBackgroundJobs(new RemoveAccountJobsModel
                    {
                        AccountId = account.Id,
                        IsForSpy = forSpy,
                        Login = account.Login
                    });
                    return false;
                }
            }

            var cookieResponse = new GetNewCookiesEngine().Execute(new GetNewCookiesModel
            {
                Login = account.Login,
                Password = account.Password,
                Driver = _seleniumManager.RegisterNewDriver(account),
                Cookie = account.Cookie
            });

            if (cookieResponse == null || cookieResponse.AuthorizationError)
            {
                if (forSpy)
                {
                    new UpdateFailSpyAccountInformationCommandHandler(new DataBaseContext()).Handle(
                    new UpdateFailSpyAccountInformationCommand
                    {
                        AccountId = account.Id,
                        AuthorizationDataIsFailed = true
                    });
                }
                else
                {
                    new UpdateFailAccountInformationCommandHandler(new DataBaseContext()).Handle(
                    new UpdateFailAccountInformationCommand
                    {
                        AccountId = account.Id,
                        AuthorizationDataIsFailed = true
                    });
                }

                backgroundJob.RemoveAccountBackgroundJobs(new RemoveAccountJobsModel
                {
                    AccountId = account.Id,
                    IsForSpy = forSpy,
                    Login = account.Login
                });

                return false;
            }

            if (cookieResponse.ConfirmationError)
            {
                if (forSpy)
                {
                    new UpdateFailSpyAccountInformationCommandHandler(new DataBaseContext()).Handle(
                    new UpdateFailSpyAccountInformationCommand
                    {
                        AccountId = account.Id,
                        ConformationIsFailed = true
                    });
                }
                else
                {
                    new UpdateFailAccountInformationCommandHandler(new DataBaseContext()).Handle(
                    new UpdateFailAccountInformationCommand
                    {
                        AccountId = account.Id,
                        ConformationIsFailed = true
                    });
                }

                backgroundJob.RemoveAccountBackgroundJobs(new RemoveAccountJobsModel
                {
                    AccountId = account.Id,
                    IsForSpy = forSpy,
                    Login = account.Login
                });

                return false;
            }

            var newCookie = cookieResponse.CookiesString;

            if (newCookie == null)
            {
                return false;
            }
            if (forSpy)
            {
                new UpdateCookiesForSpyHandler(new DataBaseContext()).Handle(new UpdateCookiesForSpyCommand
                {
                    AccountId = account.Id,
                    NewCookieString = newCookie
                });
            }
            else
            {
                new UpdateCookiesHandler(new DataBaseContext()).Handle(new UpdateCookiesCommand()
                {
                    AccountId = account.Id,
                    NewCookieString = newCookie
                });
            }

            return true;
        }
    }
}
