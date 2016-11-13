using System.Collections.Generic;

namespace Services.ViewModels.FriendMessagesModels
{
    public class FriendMessageList
    {
        public long AccountId { get; set; }

        public long FriendId { get; set; }

        public List<FriendMessage> FriendMessages { get; set; } 
    }
}
