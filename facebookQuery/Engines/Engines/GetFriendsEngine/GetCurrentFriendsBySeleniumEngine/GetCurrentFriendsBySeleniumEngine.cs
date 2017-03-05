using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using CommonModels;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;

namespace Engines.Engines.GetFriendsEngine.GetCurrentFriendsBySeleniumEngine
{
    public class GetCurrentFriendsBySeleniumEngine : AbstractEngine<GetCurrentFriendsBySeleniumModel, List<GetFriendsResponseModel>>
    {
        protected override List<GetFriendsResponseModel> ExecuteEngine(GetCurrentFriendsBySeleniumModel model)
        {
            var driver = model.Driver;
            var friendsList = new List<GetFriendsResponseModel>();

            var path = "/";
            var domain = ".facebook.com";

            var cookies = ParseCookieString(model.Cookie);

            try
            {
                foreach (var keyValuePair in cookies)
                {
                    driver.Manage()
                        .Cookies.AddCookie(new Cookie(keyValuePair.Key, keyValuePair.Value, domain, path, null));
                }

                driver.Navigate()
                    .GoToUrl(string.Format("https://www.facebook.com/profile.php?id={0}&sk=friends",
                        model.AccountFacebookId));

                Thread.Sleep(500);

                var friends = GetFriendLinks(driver);
                var currentCount = friends.Count;

                while (true)
                {
                    ScrollPage(driver);
                    friends = GetFriendLinks(driver);
                    if (friends.Count > currentCount)
                    {
                        currentCount = friends.Count;
                    }
                    else
                    {
                        break;
                    }
                }

                friends = GetFriendLinks(driver);

                foreach (var webElement in friends)
                {
                    var name = webElement.Text;
                    var id = ParseFacebookId(webElement.GetAttribute("data-gt"));
                    var uri = "https://www.facebook.com/profile.php?id=" + id;
                    friendsList.Add(new GetFriendsResponseModel
                    {
                        FacebookId = id,
                        FriendName = name,
                        Uri = uri
                    });
                }

                driver.Quit();
            }
            catch (Exception ex)
            {
                driver.Quit();
            }

            return friendsList;
        }

        private static IEnumerable<KeyValuePair<string, string>> ParseCookieString(string cookieString)
        {
            var cookiesElements = cookieString.Split(';');
            var cookiesElementsList = new List<KeyValuePair<string, string>>();

            foreach (var cookiesElement in cookiesElements)
            {
                var cookiesElementData = cookiesElement.Split('=');

                try
                {
                    cookiesElementsList.Add(new KeyValuePair<string, string>(cookiesElementData[0] ?? "", cookiesElementData[1] ?? ""));
                }
                catch (Exception)
                {

                }
            }

            return cookiesElementsList;
        }
        private static void ScrollPage(RemoteWebDriver driver)
        {
            var js = (IJavaScriptExecutor)driver;
            js.ExecuteScript(string.Format("window.scrollBy({0}, {1})", 3000, 3000), "");

            Thread.Sleep(1000);
        }


        private static IReadOnlyCollection<IWebElement> GetFriendLinks(RemoteWebDriver driver)
        {
            return driver.FindElementsByCssSelector("._6a._6b>.fsl.fwb.fcb>a");
        }

        private static long ParseFacebookId(string page)
        {
            var pattern = new Regex("eng_tid\\\":*?.\".*?.\"");
            var step1 = pattern.Match(page).ToString();

            var step2 = step1.Remove(0, 10);
            var step3 = step2.Remove(step2.Length - 1);

            return Convert.ToInt64(step3);
        }
    }
}
