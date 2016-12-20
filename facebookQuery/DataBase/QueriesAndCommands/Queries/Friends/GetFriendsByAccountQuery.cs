using System.Collections.Generic;
using CommonModels;
using DataBase.QueriesAndCommands.Models;

namespace DataBase.QueriesAndCommands.Queries.Friends
{
    public class GetFriendsByAccountQuery : IQuery<List<FriendData>>
    {
        public long AccountId { get; set; }
    }
}
