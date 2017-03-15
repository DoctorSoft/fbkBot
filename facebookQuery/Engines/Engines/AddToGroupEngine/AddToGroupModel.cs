using System.Collections.Generic;
using System.Net;
using Constants;

namespace Engines.Engines.AddToGroupEngine
{
    public class AddToGroupModel
    {
        public long FacebookId { get; set; }

        public string FacebookGroupUrl { get; set; }
        
        public string Cookie { get; set; }

        public WebProxy Proxy { get; set; }

        public List<KeyValue<int, string>> UrlParameters { get; set; }

        public List<FriendModel> FriendsList { get; set; }
    }
}
