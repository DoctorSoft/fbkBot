using System;
using Constants.GendersUnums;

namespace Engines.Engines.Models
{
    public class FacebookMessageModel
    {
        public long AccountId { get; set; }

        public long FriendFacebookId { get; set; }

        public GenderEnum Gender { get; set; }

        public string Href { get; set; }

        public string Name { get; set; }

        public int CountUnreadMessages { get; set; }

        public int CountAllMessages { get; set; }

        public string LastMessage { get; set; }

        public bool UnreadMessage { get; set; }

        public DateTime LastReadMessageDateTime { get; set; }

        public DateTime LastUnreadMessageDateTime { get; set; }
    }
}
