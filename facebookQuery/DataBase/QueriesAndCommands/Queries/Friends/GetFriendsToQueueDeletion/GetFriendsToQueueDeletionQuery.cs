using System.Collections.Generic;
using DataBase.QueriesAndCommands.Models;

namespace DataBase.QueriesAndCommands.Queries.Friends.GetFriendsToQueueDeletion
{
    public class GetFriendsToQueueDeletionQuery : IQuery<List<FriendData>>
    {
        public long AccountId { get; set; }

        public int CountFriendsToGet { get; set; }

        public int RetryNumber { get; set; }
    }
}
