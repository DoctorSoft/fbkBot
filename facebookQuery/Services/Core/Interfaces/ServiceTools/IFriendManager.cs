using DataBase.QueriesAndCommands.Models;
using DataBase.QueriesAndCommands.Queries.Friends;

namespace Services.Core.Interfaces.ServiceTools
{
    public interface IFriendManager
    {
        FriendData GetFriendByFacebookId(long friendFacebookId);

        FriendData GetFriendById(long friendAccountId);
    }
}
