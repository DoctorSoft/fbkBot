﻿using System.Net;
using OpenQA.Selenium.Remote;

namespace Engines.Engines.GetFriendsEngine.GetCurrentFriendsBySeleniumEngine
{
    public class GetCurrentFriendsBySeleniumModel
    {
        public RemoteWebDriver Driver { get; set; }
        
        public long AccountFacebookId { get; set; }

        public string Cookie { get; set; }

        public WebProxy Proxy { get; set; }

        public string UserAgent { get; set; }
    }
}
