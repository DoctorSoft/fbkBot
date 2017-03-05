using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.FriendsBlackList.ClearFriendsBlackListByGroupIdCommand;
using Services.Interfaces.ServiceTools;
using Services.ServiceTools;
using Services.ViewModels.FrindsBlackListModels;

namespace Services.Services
{
    public class FriendsBlackListServices
    {
        private readonly IFriendsBlackListManager _friendsBlackListManager;

        public FriendsBlackListServices()
        {
            _friendsBlackListManager = new FriendsBlackListManager();
        }

        public FriendsBlackListViewModel GetFriendsBlackListByGroupId(long groupId)
        {
            var friends = _friendsBlackListManager.GetFriendsBlockedListByGroupId(groupId);

            return new FriendsBlackListViewModel
            {
                GroupId = groupId,
                BlockedFriends = friends.Select(data => new BlockedFriendViewModel
                {
                    FriendFacebookId = data.FacebookId,
                    Id = data.Id,
                    FriendName = data.FriendName,
                    DateAdded = data.DateAdded
                }).ToList()
            };
        }

        public void ClearBlackList(long groupId)
        {
            new ClearFriendsBlackListByGroupIdCommandHandler(new DataBaseContext()).Handle(
                new ClearFriendsBlackListByGroupIdCommand
                {
                    GroupSettingsId = groupId
                });
        }
    }
}
