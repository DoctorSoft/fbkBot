using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;
using Services.ViewModels.HomeModels;

namespace Services.Core.Interfaces.ServiceTools
{
    public interface ISeleniumManager
    {
        RemoteWebDriver RegisterNewDriver(AccountViewModel account);
    }
}
