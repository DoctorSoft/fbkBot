using System;
using Constants.MessageEnums;

namespace Services.ViewModels.FriendMessagesModels
{
    public class FriendMessage
    {
        public long Id { get; set; }

        public string Message { get; set; }

        public DateTime? MessageDateTime { get; set; }

        public MessageDirection MessageDirection { get; set; }
    }
}
