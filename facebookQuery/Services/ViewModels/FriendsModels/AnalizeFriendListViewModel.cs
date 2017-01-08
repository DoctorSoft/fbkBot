using System.Collections.Generic;

namespace Services.ViewModels.FriendsModels
{
    public class AnalizeFriendListViewModel
    {
        public long AccountId { get; set; }

        public List<AnalizeFriendViewModel> Friends { get; set; } 
    }
}
