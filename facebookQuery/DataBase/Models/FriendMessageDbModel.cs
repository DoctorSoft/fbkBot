using System;
using Constants.MessageEnums;

namespace DataBase.Models
{
    public class FriendMessageDbModel
    {
        public long Id { get; set; }

        public string Message { get; set; }

        public DateTime MessageDateTime { get; set; }

        public FriendDbModel Friend { get; set; }

        public MessageDirection MessageDirection { get; set; }

        public long FriendId { get; set; }
    }
}
