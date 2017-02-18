using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.Friends.DeleteFriendByIdCommand;
using DataBase.QueriesAndCommands.Commands.FriendsBlackList.AddToFriendsBlackListCommand;
using DataBase.QueriesAndCommands.Models;
using DataBase.QueriesAndCommands.Queries.Friends;
using Services.Core.Interfaces.ServiceTools;

namespace Services.ServiceTools
{
    public class FriendManager : IFriendManager
    {
        public FriendData GetFriendByFacebookId(long friendFacebookId)
        {
            return new GetFriendByIdFacebookQueryHandler(new DataBaseContext()).Handle(new GetFriendByIdFacebookQuery
            {
                FacebookId = friendFacebookId
            });
        }

        public FriendData GetFriendById(long friendAccountId)
        {
            return new GetFriendByIdAccountQueryHandler(new DataBaseContext()).Handle(new GetFriendByIdAccountQuery
            {
                AccountId = friendAccountId
            });
        }

        public void AddFriendToBlackList(long groupSettingsId, long friendFacebookId)
        {
            var context = new DataBaseContext();

            var friend =
                new GetFriendByIdFacebookQueryHandler(context).Handle(new GetFriendByIdFacebookQuery
                {
                    FacebookId = friendFacebookId
                });

            new AddToFriendsBlackListCommandHandler(context).Handle(new AddToFriendsBlackListCommand
            {
                FriendFacebookId = friendFacebookId,
                FriendName = friend.FriendName,
                GroupSettingsId = groupSettingsId
            });

            new DeleteFriendByIdCommandHandler(context).Handle(new DeleteFriendByIdCommand
            {
                AccountId = friend.AccountId,
                FriendId = friend.Id
            });
        }
    }
}
