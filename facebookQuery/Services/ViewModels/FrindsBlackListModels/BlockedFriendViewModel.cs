using System;

namespace Services.ViewModels.FrindsBlackListModels
{
    public class BlockedFriendViewModel
    {
        public long Id { get; set; }

        public long FriendFacebookId { get; set; }

        public string FriendName { get; set; }

        public DateTime DateAdded { get; set; }
    }
}
