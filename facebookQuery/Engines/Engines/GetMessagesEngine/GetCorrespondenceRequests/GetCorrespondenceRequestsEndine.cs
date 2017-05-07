using System;
using System.Collections.Generic;
using System.Linq;
using Constants.FriendTypesEnum;
using Engines.Engines.Models;
using OpenQA.Selenium;

namespace Engines.Engines.GetMessagesEngine.GetCorrespondenceRequests
{
    public class GetCorrespondenceRequestsEndine : AbstractEngine<GetCorrespondenceRequestsModel, List<FacebookMessageModel>>
    {
        protected override List<FacebookMessageModel> ExecuteEngine(GetCorrespondenceRequestsModel model)
        {
            var resultList = new List<FacebookMessageModel>();

            var driver = model.Driver;

            const string path = "/";
            const string domain = ".facebook.com";

            var cookies = ParseCookieString(model.Cookie);

            try
            {
                foreach (var keyValuePair in cookies)
                {
                    driver.Manage().Cookies.AddCookie(new Cookie(keyValuePair.Key, keyValuePair.Value, domain, path, null));
                }

                driver.Navigate()
                    .GoToUrl(string.Format("https://www.facebook.com/messages/requests/t/{0}", model.AccountFacebookId));

                var friends = driver.FindElementsByCssSelector("._5l-3._1ht5").Where(element => element.GetAttribute("id").Contains("row_header_id"));
                var listId = new List<string>();

                foreach (var webElement in friends)
                {
                    var url = webElement.GetAttribute("id");
                    var id = url.Remove(0, url.IndexOf(":", StringComparison.Ordinal) + 1);
                    listId.Add(id);
                }

                var linksToDialog =
                     driver.FindElementsByCssSelector("._1ht5._2il3._5l-3")
                         .Where(element => element.GetAttribute("data-href").Contains("https://www.facebook.com/messages/requests/t")).ToList();//.Contains(friendId));
                int i = 0;
                foreach (var friendId in listId)
                {
                    var linkToDialog = linksToDialog[i];
                    i++;
                    if (linkToDialog == null)
                    {
                        continue;
                    }

                    var link = linkToDialog.GetAttribute("data-href");
                    if (link == null)
                    {
                        continue;
                    }

                    driver.Navigate().GoToUrl(link);

                    var lastMessageBlock =
                        driver.FindElementsByCssSelector(".clearfix._o46._3erg._29_7.direction_ltr.text_align_ltr")
                            .LastOrDefault();

                    var unreadMessage = lastMessageBlock != null ? lastMessageBlock.Text : "";

                    var friendNameBlock = driver.FindElementByClassName("_2jnq");
                    var friendName = friendNameBlock != null ? friendNameBlock.Text : "";

                    var messageModel = new FacebookMessageModel
                    {
                        Href = string.Format("https://www.facebook.com/profile.php?id={0}", friendId),
                        FriendFacebookId = Convert.ToInt64(friendId),
                        LastMessage = unreadMessage,
                        LastUnreadMessageDateTime = DateTime.Now,
                        Name = friendName.Remove(friendName.IndexOf("\r\n", StringComparison.Ordinal)),
                        FriendType = FriendTypes.NotFriends
                    };
                    
                    //нажимаем принять

                    var applyButton = driver.FindElementByCssSelector("._3quh._30yy._2t_");
                    if (applyButton!=null)
                    {
                        if (applyButton.Displayed)
                        {
                            applyButton.Click();

                            //сохраняем данных о друге и переписке

                            resultList.Add(messageModel); 
                        }
                    }
                }

                driver.Quit();
                return resultList;
            }
            catch (Exception ex)
            {
                driver.Quit();
            }

            return resultList;
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
    }
}
