using System.Collections.Generic;

namespace Services.ViewModels.FriendsModels
{
    public class FriendListViewModel
    {
        public List<FriendViewModel> Friends { get; set; }

        public long AccountId { get; set; }
    }
}
