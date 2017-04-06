using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.Accounts;
using DataBase.QueriesAndCommands.Commands.Cookies;
using Engines.Engines.CheckProxyEngine;
using Engines.Engines.GetNewCookiesEngine;
using OpenQA.Selenium.PhantomJS;
using Services.ViewModels.HomeModels;

namespace Services.Services
{
    public class CookieService
    {
        public bool RefreshCookies(AccountViewModel account)
        {
            if (account.Proxy != null)
            {

                var proxyIsFailed = new CheckProxyEngine().Execute(new CheckProxyModel()
                {
                    Driver = RegisterNewDriver(account)
                });

                new UpdateFailAccountInformationCommandHandler(new DataBaseContext()).Handle(
                    new UpdateFailAccountInformationCommand
                    {
                        AccountId = account.Id,
                        ProxyDataIsFailed = proxyIsFailed
                    });

                if (proxyIsFailed)
                {
                    return false;
                }
            }

            var cookieResponse = new GetNewCookiesEngine().Execute(new GetNewCookiesModel()
            {
                Login = account.Login,
                Password = account.Password,
                Driver = RegisterNewDriver(account),
                Cookie = account.Cookie
            });

            if (cookieResponse == null || cookieResponse.AuthorizationError)
            {
                new UpdateFailAccountInformationCommandHandler(new DataBaseContext()).Handle(
                new UpdateFailAccountInformationCommand
                {
                    AccountId = account.Id,
                    AuthorizationDataIsFailed = true
                });

                return false;
            }

            if (cookieResponse.ConfirmationError)
            {
                new UpdateFailAccountInformationCommandHandler(new DataBaseContext()).Handle(
                new UpdateFailAccountInformationCommand
                {
                    AccountId = account.Id,
                    ConformationIsFailed = true
                });

                return false;
            }

            var newCookie = cookieResponse.CookiesString;

            if (newCookie == null)
            {
                return false;
            }

            new UpdateCookiesHandler(new DataBaseContext()).Handle(new UpdateCookiesCommand()
            {
                AccountId = account.Id,
                NewCookieString = newCookie
            });

            return true;
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
    }
}
