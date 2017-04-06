using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using CommonModels;
using Constants;
using Constants.EnumExtension;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using RequestsHelpers;

namespace Engines.Engines.GetFriendsEngine.GetCurrentFriendsBySeleniumEngine
{
    public class GetCurrentFriendsBySeleniumEngine : AbstractEngine<GetCurrentFriendsBySeleniumModel, GetFriendsResponseModel>
    {
        private const int PercentageOfError = 4;

        protected override GetFriendsResponseModel ExecuteEngine(GetCurrentFriendsBySeleniumModel model)
        {
            var driver = model.Driver;
            var friendsList = new GetFriendsResponseModel
            {
                Friends = new List<FriendsResponseModel>()
            };

            const string path = "/";
            const string domain = ".facebook.com";

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

                Thread.Sleep(1000);

                var friends = GetFriendLinks(driver);
                var countFriendsLabel = GetFriendsCount(RequestsHelper.Get(Urls.GetFriends.GetDiscription(), model.Cookie, model.Proxy));

                var currentCount = friends.Count;

                var counter = 0;

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
                        var countPercentage = Convert.ToInt32(countFriendsLabel)/100*PercentageOfError;

                        if ((Convert.ToInt32(countFriendsLabel) - countPercentage <= currentCount) || counter > 3)
                        {
                            break;
                        }

                        counter++;
                    }
                }

                friends = GetFriendLinks(driver);
                var errorElements = new List<IWebElement>();


                foreach (var webElement in friends)
                {
                    try
                    {
                        var name = webElement.Text;
                        var id = ParseFacebookId(webElement.GetAttribute("data-gt"));
                        var uri = "https://www.facebook.com/profile.php?id=" + id;

                        var friendModels = new FriendsResponseModel
                        {
                            FacebookId = id,
                            FriendName = name,
                            Uri = uri
                        };
                        friendsList.Friends.Add(friendModels);
                    }
                    catch (Exception ex)
                    {
                        errorElements.Add(webElement);
                    }
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
        private static void ScrollPage(IJavaScriptExecutor driver)
        {
            var js = (IJavaScriptExecutor)driver;
            js.ExecuteScript(string.Format("window.scrollBy({0}, {1})", 3000, 3000), "");

            Thread.Sleep(2000);
        }

        private static IReadOnlyCollection<IWebElement> GetFriendLinks(IFindsByCssSelector driver)
        {
            try
            {
                var result = driver.FindElementsByCssSelector("._6a._6b>.fsl.fwb.fcb>a");

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static long ParseFacebookId(string page)
        {
            var pattern = new Regex("eng_tid\\\":*?.\".*?.\"");
            var step1 = pattern.Match(page).ToString();

            var step2 = step1.Remove(0, 10);
            var step3 = step2.Remove(step2.Length - 1);

            return Convert.ToInt64(step3);
        }
        public static string GetFriendsCount(string pageRequest)
        {
            var regex = new Regex("class=\"_gs6\"[^<]*");
            if (!regex.IsMatch(pageRequest)) return null;
            var collection = regex.Matches(pageRequest);
            var fullString = (from Match m in collection select m.Groups[0].Value).FirstOrDefault();

            var result = fullString != null ? fullString.Remove(0, 13) : "0";
            return result;
        }
    }
}
