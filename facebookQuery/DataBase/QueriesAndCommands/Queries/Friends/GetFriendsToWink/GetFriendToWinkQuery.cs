using System.Collections.Generic;
using DataBase.QueriesAndCommands.Models;

namespace DataBase.QueriesAndCommands.Queries.Friends.GetFriendsToWink
{
    public class GetFriendToWinkQuery : IQuery<FriendData>
    {
        public long AccountId { get; set; }

        public long GroupSettingsId { get; set; }

        public List<long> TestedFriendsId { get; set; } 
    }
}
