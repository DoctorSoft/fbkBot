using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Constants;
using Constants.EnumExtension;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using RequestsHelpers;

namespace Engines.Engines.GetFriendsEngine.GetRandomFriendFriends
{
    public class GetRandomFriendFriendsEngine : AbstractEngine<GetRandomFriendFriendsModel, List<GetRandomFriendResponseModel>>
    {
        private Random _random;

        protected override List<GetRandomFriendResponseModel> ExecuteEngine(GetRandomFriendFriendsModel model)
        {
            var driver = model.Driver;

            var result = new List<GetRandomFriendResponseModel>();
            
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
                foreach (var friendModel in model.FriendsIdList)
                {
                    _random = new Random();

                    var navigateUrl =
                        string.Format(
                            "https://www.facebook.com/{1}/friends?lst={0}%3A{1}%3A1492106232&sk=friends&source_ref=pb_friends_tl",
                            model.AccountFacebookId,
                            friendModel.FriendFacebookId);

                    driver.Navigate().GoToUrl(navigateUrl);

                    Thread.Sleep(1000);

                    var countFriendsLabel = GetFriendsCount(RequestsHelper.Get(Urls.GetFriends.GetDiscription(), model.Cookie, model.Proxy, model.UserAgent));
                    var countAllScrolls = Convert.ToInt32(countFriendsLabel) / 20;
                    var countScrolls = _random.Next(1, countAllScrolls);

                    ScrollPage(driver, countScrolls);

                    var friends = GetFriendLinks(driver).ToList();

                    var randomFriendNumber = _random.Next(0, friends.Count());

                    var id = friends[randomFriendNumber].GetAttribute("data-profileid");

                    result.Add(new GetRandomFriendResponseModel
                    {
                        FriendId = friendModel.FriendId,
                        FriendFriendFacebookId = Convert.ToInt64(id)
                    });
                }

                driver.Quit();
            }
            catch (Exception ex)
            {
                driver.Quit();
                return null;
            }

            return result;
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
        private static void ScrollPage(IJavaScriptExecutor driver, int countScrolls)
        {
            var js = (IJavaScriptExecutor)driver;
            for (int i = 0; i <= countScrolls; i++)
            {
                js.ExecuteScript(string.Format("window.scrollBy({0}, {1})", 3000, 3000), "");

                Thread.Sleep(2000);
            }
        }

        private static IEnumerable<IWebElement> GetFriendLinks(IFindsByCssSelector driver)
        {
            try
            {
                var divs = driver.FindElementsByCssSelector("._42ft._4jy0.FriendRequestOutgoing.enableFriendListFlyout.outgoingButton.enableFriendListFlyout.hidden_elem._4jy3._517h._51sy");

                return divs;
            }
            catch (Exception ex)
            {
                return null;
            }
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
