namespace Services.Core.Interfaces
{
    public interface ISendMessageCore
    {
        void SendMessageToUnread(long senderId, long friendId);

        void SendMessageToUnanswered(long senderId, long friendId);

        void SendMessageToNewFriend(long senderId, long friendId);
    }
}
