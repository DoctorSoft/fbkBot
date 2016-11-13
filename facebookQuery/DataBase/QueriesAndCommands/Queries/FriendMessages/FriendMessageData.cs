using System;
using Constants.MessageEnums;
using DataBase.Models;

namespace DataBase.QueriesAndCommands.Queries.FriendMessages
{
    public class FriendMessageData
    {
        public long Id { get; set; }

        public string Message { get; set; }

        public DateTime MessageDateTime { get; set; }

        public MessageDirection MessageDirection { get; set; }

        public long FriendId { get; set; }
    }
}
