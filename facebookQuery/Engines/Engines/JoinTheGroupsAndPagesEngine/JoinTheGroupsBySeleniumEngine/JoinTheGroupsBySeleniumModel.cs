using System.Collections.Generic;
using OpenQA.Selenium.Remote;

namespace Engines.Engines.JoinTheGroupsAndPagesEngine.JoinTheGroupsBySeleniumEngine
{
    public class JoinTheGroupsBySeleniumModel
    {
        public RemoteWebDriver Driver { get; set; }

        public string Cookie { get; set; }

        public List<string> Groups { get; set; } 
    }
}
