using DataBase.QueriesAndCommands.Queries.Account.Models;
using DataBase.QueriesAndCommands.Queries.Friends;

namespace Services.Core.Interfaces
{
    public interface ISendMessageCore
    {
        void SendMessageToUnread(AccountModel account, FriendData friend);

        void SendMessageToUnanswered(long senderId, long friendId);

        void SendMessageToNewFriend(long senderId, long friendId);
    }
}
