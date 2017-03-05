using DataBase.QueriesAndCommands.Models;
using DataBase.QueriesAndCommands.Queries.Account.Models;

namespace Services.Core.Interfaces
{
    public interface ISendMessageCore
    {
        void SendMessageToUnread(AccountModel account, FriendData friend);

        void SendMessageToUnanswered(AccountModel account, FriendData friend);

        void SendMessageToNewFriend(AccountModel account, FriendData friend);
    }
}
