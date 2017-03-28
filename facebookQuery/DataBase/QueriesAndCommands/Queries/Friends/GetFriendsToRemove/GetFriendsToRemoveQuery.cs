using System.Collections.Generic;
using DataBase.QueriesAndCommands.Models;

namespace DataBase.QueriesAndCommands.Queries.Friends.GetFriendsToRemove
{
    public class GetFriendsToRemoveQuery : IQuery<List<FriendData>>
    {
        public long AccountId { get; set; }
    }
}
