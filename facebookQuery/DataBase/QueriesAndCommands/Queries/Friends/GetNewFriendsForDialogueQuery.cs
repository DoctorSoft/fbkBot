using System.Collections.Generic;
using DataBase.QueriesAndCommands.Models;

namespace DataBase.QueriesAndCommands.Queries.Friends
{
    public class GetNewFriendsForDialogueQuery : IQuery<List<FriendData>>
    {
        public long AccountId { get; set; }

        public int DelayTime { get; set; }

        public int CountFriend { get; set; }
    }
}
