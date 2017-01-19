using OpenQA.Selenium.PhantomJS;

namespace Engines.Engines.GetFriendsEngine.GetCurrentFriendsBySeleniumEngine
{
    public class GetCurrentFriendsBySeleniumModel
    {
        public PhantomJSDriver Driver { get; set; }
        
        public long AccountFacebookId { get; set; }

        public string Cookie { get; set; }
    }
}
