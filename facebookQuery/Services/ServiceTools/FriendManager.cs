using System;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.FriendsBlackList.AddToFriendsBlackListCommand;
using DataBase.QueriesAndCommands.Models;
using DataBase.QueriesAndCommands.Queries.Friends;
using DataBase.QueriesAndCommands.Queries.Friends.GetFriendById;
using Services.Interfaces.ServiceTools;

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

        public FriendData GetFriendById(long friendId)
        {
            return new GetFriendByIdQueryHandler(new DataBaseContext()).Handle(new GetFriendByIdQuery
            {
                FriendId = friendId
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
        }

        public bool CheckConditionTime(DateTime addedDateTime, int settingsHours)
        {
            var passedTime = DateTime.Now - addedDateTime;
            var passedHour = ConvertDateTimeToHours(passedTime);
            
            return passedHour > settingsHours;
        }

        private static int ConvertDateTimeToHours(TimeSpan date)
        {
            return date.Days * 24 + date.Hours;
        }
    }
}
