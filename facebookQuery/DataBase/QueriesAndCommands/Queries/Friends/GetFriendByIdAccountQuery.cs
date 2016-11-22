using System.Collections.Generic;
using CommonModels;

namespace DataBase.QueriesAndCommands.Queries.Friends
{
    public class GetFriendByIdAccountQuery : IQuery<List<FriendData>>
    {
        public long FacebookId { get; set; }
    }
}
