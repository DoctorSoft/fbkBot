using System.Net;
using Constants.GendersUnums;

namespace Engines.Engines.GetFriendsEngine.CheckFriendInfoBySeleniumEngine
{
    public class CheckFriendGenderModel
    {
        public long AccountFacebookId { get; set; }

        public long FriendFacebookId { get; set; }

        public string Cookie { get; set; }

        public WebProxy Proxy { get; set; }
        
        public GenderEnum Gender { get; set; }
    }
}
