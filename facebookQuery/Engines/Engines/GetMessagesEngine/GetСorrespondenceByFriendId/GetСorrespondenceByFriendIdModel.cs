﻿
using System.Collections.Generic;
using Constants;

namespace Engines.Engines.GetMessagesEngine.GetСorrespondenceByFriendId
{
    public class GetСorrespondenceByFriendIdModel
    {
        public long AccountId { get; set; }

        public long FriendId { get; set; }

        public string Cookie { get; set; }

        public List<KeyValue<int, string>> UrlParameters { get; set; }
    }
}
