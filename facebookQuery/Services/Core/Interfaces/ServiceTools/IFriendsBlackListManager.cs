using System.Collections.Generic;
using DataBase.QueriesAndCommands.Models;

namespace Services.Core.Interfaces.ServiceTools
{
    public interface IFriendsBlackListManager
    {
        void ClearFriendsBlackListByGroupId(long groupSettingsId);

        List<BlockedFriendData> GetFriendsBlockedListByGroupId(long groupSettingsId);
    }
}
