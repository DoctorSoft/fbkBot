using OpenQA.Selenium.Remote;

namespace Engines.Engines.GetNewCookiesEngine
{
    public class GetNewCookiesModel
    {
        public RemoteWebDriver Driver { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string Cookie { get; set; }
    }
}
