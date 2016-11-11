namespace Engines.Engines.GetMessagesEngine.GetMessages
{
    public class GetMessagesResponseModel
    {
        public long FriendId { get; set; }

        public int CountUnreadMessages { get; set; }

        public int CountAllMessages { get; set; }

        public string LastMessage { get; set; }

        public bool UnreadMessage { get; set; }
    }
}
