using System.Net;

namespace Engines.Engines.AddToPageEngine
{
    public class BotPageIsWorkModel
    {
        public long FriendFacebookId { get; set; }

        public string Cookie { get; set; }

        public WebProxy Proxy { get; set; }
        
        public string UserAgent { get; set; }
    }
}
