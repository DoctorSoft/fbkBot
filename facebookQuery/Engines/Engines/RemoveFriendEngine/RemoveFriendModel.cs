﻿using System.Collections.Generic;
using System.Net;
using Constants;

namespace Engines.Engines.RemoveFriendEngine
{
    public class RemoveFriendModel
    {
        public long AccountFacebookId { get; set; }

        public long FriendFacebookId { get; set; }

        public string Cookie { get; set; }

        public WebProxy Proxy { get; set; }

        public List<KeyValue<int, string>> UrlParameters { get; set; }
        public string UserAgent { get; set; }
    }
}
