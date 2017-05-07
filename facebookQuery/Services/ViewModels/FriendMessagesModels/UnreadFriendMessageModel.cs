using System;
using Constants.FriendTypesEnum;
using Constants.GendersUnums;

namespace Services.ViewModels.FriendMessagesModels
{
    public class UnreadFriendMessageModel
    {
        public long FriendFacebookId { get; set; }

        public string FriendName { get; set; }

        public GenderEnum FriendGender { get; set; }

        public FriendTypes FriendType { get; set; }

        public string FriendHref { get; set; }

        public int CountUnreadMessages { get; set; }

        public int CountAllMessages { get; set; }

        public string LastMessage { get; set; }

        public bool UnreadMessage { get; set; }

        public DateTime LastReadMessageDateTime { get; set; }

        public DateTime LastUnreadMessageDateTime { get; set; }
    }
}
