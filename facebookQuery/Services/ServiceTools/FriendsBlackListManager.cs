using System.Collections.Generic;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.FriendsBlackList.ClearFriendsBlackListByGroupIdCommand;
using DataBase.QueriesAndCommands.Models;
using DataBase.QueriesAndCommands.Queries.FriendsBlackList.GetFriendsBlackList;
using Services.Core.Interfaces.ServiceTools;

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
    }
}
