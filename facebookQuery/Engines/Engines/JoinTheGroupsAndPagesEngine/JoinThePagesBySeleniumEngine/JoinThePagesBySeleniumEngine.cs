using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;

namespace Engines.Engines.JoinTheGroupsAndPagesEngine.JoinThePagesBySeleniumEngine
{
    public class JoinThePagesBySeleniumEngine : AbstractEngine<JoinThePagesBySeleniumModel, bool>
    {
        protected override bool ExecuteEngine(JoinThePagesBySeleniumModel model)
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

                foreach (var page in model.Pages)
                {
                    try
                    {
                        driver.Navigate().GoToUrl(page);
                        Thread.Sleep(2000);
                        var buttons =
                            driver.FindElements(By.CssSelector("._56bz._54k8._5c9u._5caa"));

                        if (buttons != null)
                        {
                            var firstOrDefault = buttons.FirstOrDefault(element => element.Text == "Нравится" || element.Text == "Like");
                            if (firstOrDefault != null)
                            {
                                firstOrDefault.Click();
                            }
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
