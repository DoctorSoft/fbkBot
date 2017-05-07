
using System.Collections.Generic;

namespace CommonModels
{
    public class GetFriendsResponseModel
    {
        public long CountIncommingFriends { get; set; }

        public List<FriendsResponseModel> Friends { get; set; }
    }
}
