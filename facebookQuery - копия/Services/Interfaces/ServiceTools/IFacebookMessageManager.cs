using DataBase.QueriesAndCommands.Queries.FriendMessages;

namespace Services.Interfaces.ServiceTools
{
    public interface IFacebookMessageManager
    {
        FriendMessageData GetLastFriendMessageModel(long senderId, long friendId);

        FriendMessageData GetLastBotMessageModel(long senderId, long friendId);
    }
}
