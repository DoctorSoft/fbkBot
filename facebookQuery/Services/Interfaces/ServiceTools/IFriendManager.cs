using System;
using DataBase.QueriesAndCommands.Models;

namespace Services.Interfaces.ServiceTools
{
    public interface IFriendManager
    {
        FriendData GetFriendByFacebookId(long friendFacebookId);

        FriendData GetFriendById(long friendAccountId);

        void AddFriendToBlackList(long groupSettingsId, long friendFacebookId);

        bool CheckConditionTime(DateTime addedDateTime, int settingsHours);

        bool RecountError(long currentFriendsCount, long newFriendsCount, long percent);
    }
}
