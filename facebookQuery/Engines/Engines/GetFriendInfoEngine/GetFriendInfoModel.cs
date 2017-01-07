﻿using System.Net;
using CommonModels;

namespace Engines.Engines.GetFriendInfoEngine
{
    public class GetFriendInfoModel
    {
        public long AccountFacebookId { get; set; }

        public long FriendFacebookId { get; set; }

        public string Cookie { get; set; }

        public WebProxy Proxy { get; set; }

        public AccountSettingsModel Settings { get; set; }
    }
}
