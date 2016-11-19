using System;

namespace Services.ViewModels.FriendMessagesModels
{
    public class UnreadFriendMessageModel
    {
        public long FriendId { get; set; }

        public int CountUnreadMessages { get; set; }

        public int CountAllMessages { get; set; }

        public string LastMessage { get; set; }

        public bool UnreadMessage { get; set; }

        public DateTime LastReadMessageDateTime { get; set; }

        public DateTime LastUnreadMessageDateTime { get; set; }
    }
}
