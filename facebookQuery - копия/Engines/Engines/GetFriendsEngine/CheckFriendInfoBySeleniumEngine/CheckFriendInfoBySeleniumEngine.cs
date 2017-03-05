using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace Engines.Engines.GetFriendsEngine.CheckFriendInfoBySeleniumEngine
{
    public class CheckFriendInfoBySeleniumEngine : AbstractEngine<CheckFriendInfoBySeleniumModel, bool>
    {
        protected override bool ExecuteEngine(CheckFriendInfoBySeleniumModel model)
        {
            var driver = model.Driver;
            var cities = ConvertJsonToString(model.Cities);
            var countries = ConvertJsonToString(model.Countries);

            driver.Navigate().GoToUrl("https://www.facebook.com/");

            var path = "/";
            var domain = ".facebook.com";

            var cookies = ParseCookieString(model.Cookie);

            foreach (var keyValuePair in cookies)
            {
                driver.Manage().Cookies.AddCookie(new Cookie(keyValuePair.Key, keyValuePair.Value, domain, path, null));
            }

            driver.Navigate()
                .GoToUrl(
                    string.Format(
                        "https://www.facebook.com/profile.php?id={1}&lst={0}%3A{1}%3A1485883089&sk=about",
                        model.AccountFacebookId, model.FriendFacebookId));

            Thread.Sleep(1000);
            
            var divs = driver.FindElements(By.CssSelector(".uiList._1pi3._4kg._6-h._703._4ks>li"));
            List<IWebElement> infoBlocks = null;
            if (divs != null)
            {
                infoBlocks = divs.Reverse().ToList();
                infoBlocks.RemoveAt(0);
            }

            if (infoBlocks == null)
            {
                return false;
            }

            foreach (var webElement in infoBlocks)
            {
                var linksIndiv = webElement.FindElements(By.TagName("a"));

                if (linksIndiv.Count == 0)
                {
                    continue;
                }

                var hrefs = linksIndiv.Select(element => element.GetAttribute("href")).ToList();

                var cityName = webElement.Text;

                if (!string.IsNullOrEmpty(cityName) && cities != null && CheckEntry(cities, cityName))
                {
                    return true;
                }

                foreach (var link in hrefs)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(link))
                        {
                            continue;
                        }

                        driver.Navigate().GoToUrl(link);

                        AvoidFacebookMessage(driver);

                        WaitByClassName(driver, "_3boo");

                        try
                        {
                            var countryName = driver.FindElement(By.ClassName("_3boo"));

                            if (countryName != null && !string.IsNullOrEmpty(countryName.Text))
                            {
                                if ((countries != null && (CheckEntry(countries, countryName.Text))) || (cities!=null && CheckEntry(cities, countryName.Text)))
                                {
                                    return true;
                                }
                            }
                        }
                        catch (Exception)
                        {
                            try
                            {
                                WaitByClassName(driver, "_5a_");

                                var countryDiv = driver.FindElement(By.ClassName("_5a_"));
                                var countryTagLink = countryDiv.FindElement(By.TagName("a"));

                                if ((countries != null && (CheckEntry(countries, countryTagLink.Text))) || (cities != null && CheckEntry(cities, countryTagLink.Text)))
                                {
                                    return true;
                                }

                                var countryHref = countryTagLink.GetAttribute("href");


                                if (countryHref != null)
                                {
                                    driver.Navigate().GoToUrl(countryHref);

                                    AvoidFacebookMessage(driver);

                                    var countryName = driver.FindElement(By.ClassName("_3boo"));

                                    if (countryName != null && !string.IsNullOrEmpty(countryName.Text))
                                    {
                                        if ((countries != null && (CheckEntry(countries, countryName.Text))) ||
                                            (cities != null && CheckEntry(cities, countryName.Text)))
                                        {
                                            return true;
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                
                            }
                        }

                    }
                    catch (Exception ex)
                    {

                    }
                }

                break;
            }

            return false;
        }

        private static void WaitByClassName(RemoteWebDriver driver, string className)
        {
            try
            {
                new WebDriverWait(driver, TimeSpan.FromSeconds(2)).Until(
                    ExpectedConditions.ElementIsVisible((By.ClassName(className))));
            }
            catch (Exception ex)
            {

            }
        }

        private List<string> ConvertJsonToString(string jsonData)
        {
            try
            {
                var words = JsonConvert.DeserializeObject<List<string>>(jsonData);

                return words;
            }
            catch (Exception ex)
            {
                return null;
            }
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

        private static bool CheckEntry(IEnumerable<string> list, string checkData)
        {
            var upperList = list.Select(element => element.ToUpper()).ToList();

            var entry = upperList.Any(element => checkData.ToUpper().Contains(element));

            return entry;
        }

        protected void AvoidFacebookMessage(RemoteWebDriver driver)
        {
            Thread.Sleep(500);
            try
            {
                var modalBackground = driver.FindElement(By.ClassName("_3ixn"));
                if (modalBackground != null)
                {
                    SendKeys.SendWait("{Tab}");
                    SendKeys.SendWait("{Enter}");

                    modalBackground.Click();
                }

            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
