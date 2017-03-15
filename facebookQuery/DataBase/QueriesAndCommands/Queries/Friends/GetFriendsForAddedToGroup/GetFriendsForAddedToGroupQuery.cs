using System.Collections.Generic;
using DataBase.QueriesAndCommands.Models;

namespace DataBase.QueriesAndCommands.Queries.Friends.GetFriendsForAddedToGroup
{
    public class GetFriendsForAddedToGroupQuery : IQuery<List<FriendData>>
    {
        public long AccountId { get; set; }

        public int Count { get; set; }
    }
}
