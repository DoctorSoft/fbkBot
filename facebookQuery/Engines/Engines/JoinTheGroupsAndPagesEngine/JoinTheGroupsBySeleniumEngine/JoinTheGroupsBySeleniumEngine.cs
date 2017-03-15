using System;
using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium;

namespace Engines.Engines.JoinTheGroupsAndPagesEngine.JoinTheGroupsBySeleniumEngine
{
    public class JoinTheGroupsBySeleniumEngine : AbstractEngine<JoinTheGroupsBySeleniumModel, bool>
    {
        protected override bool ExecuteEngine(JoinTheGroupsBySeleniumModel model)
        {
            var driver = model.Driver;

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

                foreach (var group in model.Groups)
                {
                    try
                    {
                        driver.Navigate().GoToUrl(group);
                        Thread.Sleep(1000);

                        var button =
                            driver.FindElement(By.CssSelector("._42ft._4jy0._3o9h._3o9h._4jy4._4jy2.selected._51sy"));

                        if (button != null)
                        {
                            button.Click();
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }

                driver.Quit();

                return true;
            }
            catch
            {
                driver.Quit();
                return false;
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
                    cookiesElementsList.Add(new KeyValuePair<string, string>(cookiesElementData[0] ?? "",
                        cookiesElementData[1] ?? ""));
                }
                catch (Exception)
                {

                }
            }

            return cookiesElementsList;
        }
    }
}
