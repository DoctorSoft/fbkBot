using System.Collections.Generic;

namespace DataBase.QueriesAndCommands.Queries.Friends.GetFriendsToWinkFriendsFriends
{
    public class GetFriendsToWinkFriendsFriendsQuery : IQuery<List<GetFriendsToWinkFriendsFriendsResponseModel>>
    {
        public long AccountId { get; set; }

        public long GroupSettingsId { get; set; }

        public int CountFriends { get; set; }
    }
}
