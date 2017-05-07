using System.Collections.Generic;
using System.Net;
using Constants;
using Engines.Engines.AddToGroupEngine;

namespace Engines.Engines.AddToPageEngine
{
    public class AddToPageModel
    {
        public long FacebookId { get; set; }

        public string FacebookPageUrl { get; set; }
        
        public string Cookie { get; set; }

        public WebProxy Proxy { get; set; }

        public List<KeyValue<int, string>> UrlParameters { get; set; }

        public FriendModel Friend { get; set; }
        public string UserAgent { get; set; }
    }
}
