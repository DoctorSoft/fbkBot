using System.Collections.Generic;

namespace Services.ViewModels.FrindsBlackListModels
{
    public class FriendsBlackListViewModel
    {
        public long GroupId { get; set; }

        public List<BlockedFriendViewModel> BlockedFriends { get; set; }
    }
}
