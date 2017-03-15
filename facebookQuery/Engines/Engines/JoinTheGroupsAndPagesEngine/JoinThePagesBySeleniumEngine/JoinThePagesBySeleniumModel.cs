using System.Collections.Generic;
using OpenQA.Selenium.Remote;

namespace Engines.Engines.JoinTheGroupsAndPagesEngine.JoinThePagesBySeleniumEngine
{
    public class JoinThePagesBySeleniumModel
    {
        public RemoteWebDriver Driver { get; set; }

        public string Cookie { get; set; }

        public List<string> Pages { get; set; } 
    }
}
