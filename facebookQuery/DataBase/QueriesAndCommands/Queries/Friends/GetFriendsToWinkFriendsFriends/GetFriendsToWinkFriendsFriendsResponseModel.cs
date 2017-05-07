using System.Collections.Generic;

namespace DataBase.QueriesAndCommands.Queries.Friends.GetFriendsToWinkFriendsFriends
{
    public class GetFriendsToWinkFriendsFriendsResponseModel : IQuery<List<long>>
    {
        public long FriendId { get; set; }

        public long FriendFacebookId { get; set; }
    }
}
