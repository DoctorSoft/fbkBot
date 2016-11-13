namespace Services.ViewModels.MessagesModels
{
    public class UnreadMessageModel
    {
        public long FacebookFriendId { get; set; }

        public int CountUnreadMessages { get; set; }

        public int CountAllMessages { get; set; }

        public string LastMessage { get; set; }

        public bool UnreadMessage { get; set; }
    }
}
