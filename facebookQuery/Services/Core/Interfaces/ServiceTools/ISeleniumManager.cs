using OpenQA.Selenium.PhantomJS;
using Services.ViewModels.HomeModels;

namespace Services.Core.Interfaces.ServiceTools
{
    public interface ISeleniumManager
    {
        PhantomJSDriver RegisterNewDriver(AccountViewModel account);
    }
}
