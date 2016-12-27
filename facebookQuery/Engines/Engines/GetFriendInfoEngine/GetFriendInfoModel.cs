using System.Net;

namespace Engines.Engines.GetFriendInfoEngine
{
    public class GetFriendInfoModel
    {
        public long AccountFacebookId { get; set; }

        public long FrienFacebookId { get; set; }

        public string Cookie { get; set; }

        public WebProxy Proxy { get; set; }

        public bool GetGenderFunctionEnable { get; set; }
    }
}
