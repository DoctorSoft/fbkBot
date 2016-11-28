using System.Collections.Generic;
using CommonModels;

namespace DataBase.QueriesAndCommands.Queries.Friends
{
    public class GetFriendByIdAccountQuery : IQuery<FriendData>
    {
        public long AccountId { get; set; }
    }
}
