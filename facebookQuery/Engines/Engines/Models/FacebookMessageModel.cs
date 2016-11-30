using System;

namespace Engines.Engines.Models
{
    public class FacebookMessageModel
    {
        public long AccountId { get; set; }

        public long FriendId { get; set; }

        public int CountUnreadMessages { get; set; }

        public int CountAllMessages { get; set; }

        public string LastMessage { get; set; }

        public bool UnreadMessage { get; set; }

        public DateTime LastReadMessageDateTime { get; set; }

        public DateTime LastUnreadMessageDateTime { get; set; }
    }
}
