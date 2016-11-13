using System.Collections.Generic;
using CommonModels;

namespace DataBase.QueriesAndCommands.Queries.Friends
{
    public class GetFriendsByAccountQuery : IQuery<List<FriendData>>
    {
        public long AccountId { get; set; }
    }
}
