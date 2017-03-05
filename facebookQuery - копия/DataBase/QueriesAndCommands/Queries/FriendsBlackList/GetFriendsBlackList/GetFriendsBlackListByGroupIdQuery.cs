using System.Collections.Generic;
using DataBase.QueriesAndCommands.Models;

namespace DataBase.QueriesAndCommands.Queries.FriendsBlackList.GetFriendsBlackList
{
    public class GetFriendsBlackListByGroupIdQuery : IQuery<List<BlockedFriendData>>
    {
        public long GroupSettingsId { get; set; }
    }
}
