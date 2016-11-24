using System.Collections.Generic;

namespace DataBase.QueriesAndCommands.Queries.Friends
{
    public class GetNewFriendsForDialogueQuery : IQuery<List<FriendData>>
    {
        public long AccountId { get; set; }

        public int DelayTime { get; set; }
    }
}
