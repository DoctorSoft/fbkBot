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

            var newCookie = new GetNewCookiesEngine().Execute(new GetNewCookiesModel()
            {
                Login = account.Login,
                Password = account.Password,
                Driver = RegisterNewDriver(account),
                Cookie = account.Cookie
            }).CookiesString;

            new UpdateFailAccountInformationCommandHandler(new DataBaseContext()).Handle(
            new UpdateFailAccountInformationCommand
            {
                AccountId = account.Id,
                AuthorizationDataIsFailed = newCookie == null
            });

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
