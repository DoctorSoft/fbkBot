using System.Collections.Generic;
using DataBase.QueriesAndCommands.Models;

namespace Services.Interfaces.ServiceTools
{
    public interface IFriendsBlackListManager
    {
        void ClearFriendsBlackListByGroupId(long groupSettingsId);

        List<BlockedFriendData> GetFriendsBlockedListByGroupId(long groupSettingsId);

        bool CheckForFriendBlacklist(long friendFacebookId, long groupSettingsId);
    }
}
