using System.Collections.Generic;

namespace Services.ViewModels.FriendsModels
{
    public class NewFriendListViewModel
    {
        public List<NewFriendViewModel> NewFriends { get; set; }

        public long AccountId { get; set; }
    }
}
