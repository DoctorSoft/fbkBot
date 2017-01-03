using System.Collections.Generic;
using System.Net;
using Constants;

namespace Engines.Engines.GetFriendsEngine.AddFriendEngine
{
    public class AddFriendModel
    {
        public long AccountFacebookId { get; set; }

        public long FriendFacebookId { get; set; }
        
        public string Cookie { get; set; }

        public WebProxy Proxy { get; set; }

        public List<KeyValue<int, string>> AddFriendUrlParameters { get; set; }

        public List<KeyValue<int, string>> AddFriendExtraUrlParameters { get; set; }
    }
}
