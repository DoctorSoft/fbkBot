﻿using System;
using Constants.MessageEnums;

namespace DataBase.QueriesAndCommands.Queries.FriendMessages
{
    public class FriendMessageData
    {
        public long Id { get; set; }

        public long FriendId { get; set; }

        public long FriendFacebookId { get; set; }

        public string Message { get; set; }

        public DateTime MessageDateTime { get; set; }

        public int OrderNumber { get; set; }

        public MessageDirection MessageDirection { get; set; }
    }
}
