using System.Linq;
using System.Threading;
using OpenQA.Selenium.Chrome;

namespace Engines.Engines.GetNewCookiesEngine
{
    public class GetNewCookiesEngine : AbstractEngine<GetNewCookiesModel, GetNewCookiesResponse>
    {
        protected override GetNewCookiesResponse ExecuteEngine(GetNewCookiesModel model)
        {
            var driver = new ChromeDriver();

            driver.Navigate().GoToUrl("https://www.facebook.com/login.php?login_attempt=1&lwv=110");

            Thread.Sleep(500);

            var email = driver.FindElementById("email");
            Thread.Sleep(300);
            var pass = driver.FindElementById("pass");
            Thread.Sleep(300);
            var button = driver.FindElementById("loginbutton");

            email.SendKeys(model.Login);
            pass.SendKeys(model.Password);
            Thread.Sleep(300);

            button.Click();

            var cookies = driver.Manage().Cookies;
            var cookiesResult = cookies.AllCookies.Aggregate("", (current, cookie) => current + (cookie.Name + "=" + cookie.Value + ";"));

            driver.Close();

            return new GetNewCookiesResponse
            {
                CookiesString = cookiesResult
            };
        }
    }
}
