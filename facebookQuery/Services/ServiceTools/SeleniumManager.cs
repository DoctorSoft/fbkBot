using DataBase.Context;
using DataBase.QueriesAndCommands.Queries.UserAgent.GetUserAgentById;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;
using Services.Interfaces.ServiceTools;
using Services.ViewModels.HomeModels;

namespace Services.ServiceTools
{
    public class SeleniumManager : ISeleniumManager
    {
        public RemoteWebDriver RegisterNewDriver(AccountViewModel account)
        {
            var options = new PhantomJSOptions();
            var userAgent = new GetUserAgentQueryHandler(new DataBaseContext()).Handle(new GetUserAgentQuery
            {
                UserAgentId = account.UserAgentId
            });

            if (string.IsNullOrWhiteSpace(account.Proxy))
            {
                options.AddAdditionalCapability("phantomjs.page.settings.userAgent", userAgent);
                return new PhantomJSDriver(options);
            }

            var service = PhantomJSDriverService.CreateDefaultService();
            service.AddArgument(string.Format("--proxy-auth={0}:{1}", account.ProxyLogin, account.ProxyPassword));
            service.AddArgument(string.Format("--proxy={0}", account.Proxy));

            options.AddAdditionalCapability("phantomjs.page.settings.userAgent", userAgent);

            var driver = new PhantomJSDriver(service, options);

            return driver;
        }
    }
}
