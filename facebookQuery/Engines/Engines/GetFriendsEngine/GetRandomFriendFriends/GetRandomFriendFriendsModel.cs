using System.Collections.Generic;
using System.Net;
using OpenQA.Selenium.Remote;

namespace Engines.Engines.GetFriendsEngine.GetRandomFriendFriends
{
    public class GetRandomFriendFriendsModel
    {
        public RemoteWebDriver Driver { get; set; }
        
        public long AccountFacebookId { get; set; }

        public string Cookie { get; set; }

        public WebProxy Proxy { get; set; }

        public List<GetRandomFriendModel> FriendsIdList { get; set; }
        public string UserAgent { get; set; }
    }
}
