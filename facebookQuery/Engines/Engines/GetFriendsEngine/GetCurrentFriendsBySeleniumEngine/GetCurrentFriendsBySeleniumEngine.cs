using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using CommonModels;
using Constants;
using Constants.EnumExtension;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using RequestsHelpers;

namespace Engines.Engines.GetFriendsEngine.GetCurrentFriendsBySeleniumEngine
{
    public class GetCurrentFriendsBySeleniumEngine : AbstractEngine<GetCurrentFriendsBySeleniumModel, List<FriendsResponseModel>>
    {
        private const int PercentageOfError = 4;

        protected override List<FriendsResponseModel> ExecuteEngine(GetCurrentFriendsBySeleniumModel model)
        {
            var driver = model.Driver;
            var friendsList = new List<FriendsResponseModel>();

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
                    .GoToUrl(string.Format("https://facebook.com/{0}/friends?lst={0}%3A{0}%3A1492727311",
                        model.AccountFacebookId));
            
                Thread.Sleep(1000);

                var friends = GetFriendLinks(driver);
                var countFriendsLabel = GetFriendsCount(RequestsHelper.Get(Urls.GetFriends.GetDiscription(), model.Cookie, model.Proxy, model.UserAgent));

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
                        var nameElement = webElement.FindElement(By.CssSelector(".fsl.fwb.fcb"));
                        var name = nameElement != null ? nameElement.Text : "";

                        var idElement = webElement.FindElement(By.CssSelector(".fsl.fwb.fcb>a"));
                        var attr = idElement.GetAttribute("data-gt");
                        
                        var id = ParseFacebookId(attr);
                        var uri = "https://www.facebook.com/profile.php?id=" + id;

                        var friendModels = new FriendsResponseModel
                        {
                            FacebookId = Convert.ToInt64(id),
                            FriendName = name,
                            Uri = uri
                        };
                        friendsList.Add(friendModels);
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
                //var result = driver.FindElementsByCssSelector("._55wo._55x2>._55wq._4g33._5pxa");
                var result = driver.FindElementsByCssSelector(".uiList._262m._4kg>._698");
                
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static string ParseFacebookId(string page)
        {
            var data = (JObject)JsonConvert.DeserializeObject(page);
            var s = (JObject)JsonConvert.DeserializeObject(data.GetValue("engagement").ToString());
            var id = s.GetValue("eng_tid");
            return id.ToString();
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
