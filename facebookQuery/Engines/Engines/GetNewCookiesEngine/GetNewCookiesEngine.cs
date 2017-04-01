using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;

namespace Engines.Engines.GetNewCookiesEngine
{
    public class GetNewCookiesEngine : AbstractEngine<GetNewCookiesModel, GetNewCookiesResponse>
    {
        protected override GetNewCookiesResponse ExecuteEngine(GetNewCookiesModel model)
        {
            var driver = model.Driver;
            const string path = "/";
            const string domain = ".facebook.com";

            string cookiesResult = null;

            try
            {
                driver.Navigate().GoToUrl("https://www.facebook.com");

                var currentCookies = ParseCookieString(model.Cookie);
                
                if (currentCookies != null)
                {
                    foreach (var keyValuePair in currentCookies)
                    {
                        driver.Manage()
                            .Cookies.AddCookie(new Cookie(keyValuePair.Key, keyValuePair.Value, domain, path, null));
                    }
                }

                driver.Navigate().GoToUrl("https://www.facebook.com/login.php?login_attempt=1&lwv=110");

                Thread.Sleep(500);

                IWebElement email = null;
                IWebElement pass = null;
                IWebElement button = null;
                try
                {
                    email = driver.FindElementById("email");
                    pass = driver.FindElementById("pass");
                    button = driver.FindElementById("loginbutton");
                }
                catch (Exception)
                {
                }

                ICookieJar cookies;
                string currentUri;

                if (email == null && pass == null && button == null)
                {
                    driver.Navigate().Refresh();
                    
                    currentUri = driver.Url;
                    if (currentUri.Contains("checkpoint"))
                    {
                        driver.Quit();
                        return new GetNewCookiesResponse
                        {
                            ConfirmationError = true
                        };
                    }

                    cookies = driver.Manage().Cookies;

                    if (cookies.AllCookies.Count == 0)
                    {
                        //error get cookies
                        driver.Quit();
                        return null;
                    }

                    cookiesResult = cookies.AllCookies.Aggregate("",
                        (current, cookie) => current + (cookie.Name + "=" + cookie.Value + ";"));

                    driver.Quit();

                    return new GetNewCookiesResponse
                    {
                        CookiesString = cookiesResult
                    };
                }

                email.SendKeys(model.Login);
                pass.SendKeys(model.Password);
                Thread.Sleep(300);

                button.Click();

                currentUri = driver.Url;
                if (currentUri.Contains("checkpoint"))
                {
                    driver.Quit();
                    return new GetNewCookiesResponse
                    {
                        ConfirmationError = true
                    };
                }

                cookies = driver.Manage().Cookies;

                if (cookies.AllCookies.Count == 0)
                {
                    //error get cookies
                    driver.Quit();
                    return new GetNewCookiesResponse
                    {
                        AuthorizationError = true
                    };
                }

                cookiesResult = cookies.AllCookies.Aggregate("",
                    (current, cookie) => current + (cookie.Name + "=" + cookie.Value + ";"));

                driver.Quit();

                return new GetNewCookiesResponse
                {
                    CookiesString = cookiesResult
                };
            }
            catch (Exception ex)
            {
                driver.Quit();
            }

            driver.Quit();
            return new GetNewCookiesResponse
            {
                CookiesString = cookiesResult
            };
        }

        private static IEnumerable<KeyValuePair<string, string>> ParseCookieString(string cookieString)
        {
            if (cookieString == null)
            {
                return null;    
            }

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
