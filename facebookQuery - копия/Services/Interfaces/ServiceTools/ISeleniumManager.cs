using OpenQA.Selenium.Remote;
using Services.ViewModels.HomeModels;

namespace Services.Interfaces.ServiceTools
{
    public interface ISeleniumManager
    {
        RemoteWebDriver RegisterNewDriver(AccountViewModel account);
    }
}
