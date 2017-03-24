using System;

namespace Services.ViewModels.FriendsModels
{
    public class FriendViewModel
    {
        public long FacebookId { get; set; }

        public string Name { get; set; }

        public bool Deleted { get; set; }

        public long Id { get; set; }

        public bool MessagesEnded { get; set; }

        public DateTime AddDateTime { get; set; }
    }
}
