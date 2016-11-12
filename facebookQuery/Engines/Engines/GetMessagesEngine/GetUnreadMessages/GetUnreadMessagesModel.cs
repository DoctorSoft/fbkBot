﻿using System.Collections.Generic;
using Constants;

namespace Engines.Engines.GetMessagesEngine.GetUnreadMessages
{
    public class GetUnreadMessagesModel
    {
        public long AccountId { get; set; }

        public string Cookie { get; set; }

        public List<KeyValue<int, string>> UrlParameters { get; set; }
    }
}
