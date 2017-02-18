using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;
using Services.Core.Interfaces.ServiceTools;
using Services.ViewModels.HomeModels;

namespace Services.ServiceTools
{
    public class SeleniumManager : ISeleniumManager
    {
        public RemoteWebDriver RegisterNewDriver(AccountViewModel account)
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
