using System.Collections.Generic;
using DataBase.QueriesAndCommands.Models;

namespace DataBase.QueriesAndCommands.Queries.Friends
{
    public class GetFriendsByAccountQuery : IQuery<FriendListForPaging>
    {
        public long AccountId { get; set; }

        public PageModel Page { get; set; }
    }
}
