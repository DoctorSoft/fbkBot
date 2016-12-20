using System.Collections.Generic;
using CommonModels;
using DataBase.QueriesAndCommands.Models;

namespace DataBase.QueriesAndCommands.Queries.Friends
{
    public class GetFriendByIdAccountQuery : IQuery<FriendData>
    {
        public long AccountId { get; set; }
    }
}
