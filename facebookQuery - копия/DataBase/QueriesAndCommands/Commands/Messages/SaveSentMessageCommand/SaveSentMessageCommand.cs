using System;
using Constants.MessageEnums;

namespace DataBase.QueriesAndCommands.Commands.Messages.SaveSentMessageCommand
{
    public class SaveSentMessageCommand : IVoidCommand
    {
        public long AccountId { get; set; }
        
        public long FriendId { get; set; }

        public string Message { get; set; }

        public int OrderNumber { get; set; }

        public MessageRegime MessageRegime { get; set; }
        
        public DateTime MessageDateTime { get; set; }

    }
}
