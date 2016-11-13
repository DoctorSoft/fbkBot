using System.Collections.Generic;

namespace DataBase.QueriesAndCommands.Queries.FriendMessages
{
    public class GetFriendMessagesQuery : IQuery<List<FriendMessageData>>
    {
        public long AccountId { get; set; }

        public long FriendId { get; set; }
    }
}
