using System.Collections.Generic;
using CommonModels;

namespace DataBase.QueriesAndCommands.Queries.Friends
{
    public class GetFriendsByAccountQuery : IQuery<List<GetFriendsResponseModel>>
    {
        public long AccountId { get; set; }
    }
}
