using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Models;

namespace DataBase.QueriesAndCommands.Queries.Friends.GetFriendsToWink
{
    public class GetFriendToWinkQueryHandler : IQueryHandler<GetFriendToWinkQuery, FriendData>
    {
        private readonly DataBaseContext _context;

        public GetFriendToWinkQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public FriendData Handle(GetFriendToWinkQuery query)
        {
            var friend = _context.Friends
                .Where(model => model.AccountId == query.AccountId)
                .Where(model => model.AddedToRemoveDateTime == null)//не добавлен к удалению
                .Where(model => !model.IsWinked)//не подмигивали
                .FirstOrDefault(model => !_context.FriendsBlackList.Any(dbModel => dbModel.FriendFacebookId == model.FacebookId && dbModel.GroupId == query.GroupSettingsId));//не в чс

            if (friend == null)
            {
                return null;
            }

            var friendData = new FriendData
            {
                Id = friend.Id,
                AccountId = friend.AccountId,
                AddedDateTime = friend.AddedDateTime,
                IsAddedToGroups = friend.IsAddedToGroups,
                IsAddedToPages = friend.IsAddedToPages,
                IsWinked = friend.IsWinked,
                Href = friend.Href,
                AddedToRemoveDateTime = friend.AddedToRemoveDateTime,
                Deleted = friend.DeleteFromFriends,
                FacebookId = friend.FacebookId,
                Gender = friend.Gender,
                MessageRegime = friend.MessageRegime,
                IsWinkedFriendsFriend = friend.IsWinkedFriendsFriend,
                DialogIsCompleted = friend.DialogIsCompleted,
                FriendName = friend.FriendName
            };

            return friendData;
        }
    }
}
