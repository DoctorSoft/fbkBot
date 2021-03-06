﻿using System.Collections.Generic;
using System.Net;
using Constants;

namespace Engines.Engines.SendMessageEngine
{
    public class SendMessageModel
    {
        public long AccountId { get; set; }

        public long FriendId { get; set; }

        public string Message { get; set; }

        public string Cookie { get; set; }

        public WebProxy Proxy { get; set; }

        public List<KeyValue<int, string>> UrlParameters { get; set; }

        public string UserAgent { get; set; }
    }
}
