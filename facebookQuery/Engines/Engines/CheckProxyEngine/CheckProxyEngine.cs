using System;
using System.Threading;
using OpenQA.Selenium;

namespace Engines.Engines.CheckProxyEngine
{
    public class CheckProxyEngine : AbstractEngine<CheckProxyModel, bool>
    {
        protected override bool ExecuteEngine(CheckProxyModel model)
        {
            var driver = model.Driver;

            try
            {
                driver.Navigate().GoToUrl("https://www.google.com");

                Thread.Sleep(3000);

                var element = driver.FindElement(By.Id("hplogo"));

                if (element != null)
                {
                    driver.Quit();
                    return false;
                }
            }
            catch (Exception)
            {
                driver.Quit();
                return true;
            }

            driver.Quit();
            return true;
        }
    }
}
