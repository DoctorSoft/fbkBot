using System.Collections.Generic;
using System.Net;
using Constants;

namespace Engines.Engines.GetMessagesEngine.GetUnreadMessages
{
    public class GetUnreadMessagesModel
    {
        public long AccountId { get; set; }

        public string Cookie { get; set; }

        public WebProxy Proxy { get; set; }

        public List<KeyValue<int, string>> UrlParameters { get; set; }
        public string UserAgent { get; set; }
    }
}
