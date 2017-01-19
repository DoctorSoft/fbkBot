﻿using System.Collections.Generic;

namespace DataBase.QueriesAndCommands.Queries.FriendMessages
{
    public class GetUnansweredMessagesQuery : IQuery<List<FriendMessageData>>
    {
        public long AccountId { get; set; }
        
        public long DelayTime { get; set; }
    }
}
