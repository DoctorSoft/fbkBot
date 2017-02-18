using DataBase.QueriesAndCommands.Models;

namespace Services.Core.Interfaces.ServiceTools
{
    public interface IFriendManager
    {
        FriendData GetFriendByFacebookId(long friendFacebookId);

        FriendData GetFriendById(long friendAccountId);

        void AddFriendToBlackList(long groupSettingsId, long friendFacebookId);
    }
}
