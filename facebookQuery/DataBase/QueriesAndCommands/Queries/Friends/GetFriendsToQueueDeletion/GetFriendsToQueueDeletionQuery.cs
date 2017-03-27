using System.Collections.Generic;
using DataBase.QueriesAndCommands.Models;

namespace DataBase.QueriesAndCommands.Queries.Friends.GetFriendsToQueueDeletion
{
    public class GetFriendsToQueueDeletionQuery : IQuery<List<FriendData>>
    {
        public long AccountId { get; set; }
    }
}
