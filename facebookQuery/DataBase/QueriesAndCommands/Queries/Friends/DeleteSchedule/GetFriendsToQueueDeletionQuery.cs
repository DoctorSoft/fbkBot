using System.Collections.Generic;
using DataBase.QueriesAndCommands.Models;

namespace DataBase.QueriesAndCommands.Queries.Friends.DeleteSchedule
{
    public class GetFriendsToQueueDeletionQuery : IQuery<List<FriendData>>
    {
        public long AccountId { get; set; }
    }
}
