using OpenQA.Selenium.Remote;

namespace Engines.Engines.GetFriendsEngine.CheckFriendInfoBySeleniumEngine
{
    public class CheckFriendInfoBySeleniumModel
    {
        public RemoteWebDriver Driver { get; set; }

        public long AccountFacebookId { get; set; }

        public long FriendFacebookId { get; set; }

        public string Countries { get; set; }

        public string Cities { get; set; }

        public string Cookie { get; set; }
    }
}
