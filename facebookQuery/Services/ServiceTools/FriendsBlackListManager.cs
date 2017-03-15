using System.Collections.Generic;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.FriendsBlackList.ClearFriendsBlackListByGroupIdCommand;
using DataBase.QueriesAndCommands.Models;
using DataBase.QueriesAndCommands.Queries.FriendsBlackList.CheckForFriendBlacklisted;
using DataBase.QueriesAndCommands.Queries.FriendsBlackList.GetFriendsBlackList;
using Services.Interfaces.ServiceTools;

namespace Services.ServiceTools
{
    public class FriendsBlackListManager : IFriendsBlackListManager
    {
        public void ClearFriendsBlackListByGroupId(long groupSettingsId)
        {
            new ClearFriendsBlackListByGroupIdCommandHandler(new DataBaseContext()).Handle(
                new ClearFriendsBlackListByGroupIdCommand
                {
                    GroupSettingsId = groupSettingsId
                });
        }

        public List<BlockedFriendData> GetFriendsBlockedListByGroupId(long groupSettingsId)
        {
            var friendList =
                new GetFriendsBlackListByGroupIdQueryHandler(new DataBaseContext()).Handle(
                    new GetFriendsBlackListByGroupIdQuery
                    {
                        GroupSettingsId = groupSettingsId
                    });

            return friendList;
        }

        public bool CheckForFriendBlacklist(long friendFacebookId, long groupSettingsId)
        {
            var result =
                new CheckForFriendBlacklistedQueryHandler().Handle(
                    new CheckForFriendBlacklistedQuery
                    {
                        GroupSettingsId = groupSettingsId,
                        FriendFacebookId = friendFacebookId
                    });

            return result;
        }
    }
}
