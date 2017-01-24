using System.Collections.Generic;
using System.Net;
using Constants;

namespace Engines.Engines.CancelFriendshipRequestEngine
{
    public class CancelFriendshipRequestModel
    {
        public long AccountFacebookId { get; set; }

        public long FriendFacebookId { get; set; }

        public string Cookie { get; set; }

        public WebProxy Proxy { get; set; }

        public List<KeyValue<int, string>> UrlParameters { get; set; }
    }
}
