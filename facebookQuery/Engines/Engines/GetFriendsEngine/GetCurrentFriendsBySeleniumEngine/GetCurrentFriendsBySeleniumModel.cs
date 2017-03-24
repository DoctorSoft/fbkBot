using OpenQA.Selenium.Remote;

namespace Engines.Engines.GetFriendsEngine.GetCurrentFriendsBySeleniumEngine
{
    public class GetCurrentFriendsBySeleniumModel
    {
        public RemoteWebDriver Driver { get; set; }
        
        public long AccountFacebookId { get; set; }

        public string Cookie { get; set; }
    }
}
