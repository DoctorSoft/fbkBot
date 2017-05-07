using DataBase.QueriesAndCommands.Models;
using Services.ViewModels.HomeModels;

namespace Services.Core.Interfaces
{
    public interface ISendMessageCore
    {
        void SendMessageToUnread(AccountViewModel account, FriendData friend);

        void SendMessageToUnanswered(AccountViewModel account, FriendData friend);

        void SendMessageToNewFriend(AccountViewModel account, FriendData friend);
    }
}
