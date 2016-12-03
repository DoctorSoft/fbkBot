using OpenQA.Selenium.PhantomJS;

namespace Engines.Engines.GetNewCookiesEngine
{
    public class GetNewCookiesModel
    {
        public PhantomJSDriver Driver { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }
    }
}
