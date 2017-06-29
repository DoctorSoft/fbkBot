using System.Collections.Generic;
using Services.ViewModels.PageModels;

namespace Services.ViewModels.FriendsModels
{
    public class FriendListViewModel
    {
        public List<FriendViewModel> Friends { get; set; }

        public long AccountId { get; set; }

        public PageInfoModel PageInfo { get; set; }
    }
}
