using System.Net;

namespace Engines.Engines.GetFriendsCountEngine
{
    public class GetFriendsCountModel
    {
        public long AccountFacebookId { get; set; }

        public string Cookie { get; set; }

        public WebProxy Proxy { get; set; }
        public string UserAgent { get; set; }
    }
}
