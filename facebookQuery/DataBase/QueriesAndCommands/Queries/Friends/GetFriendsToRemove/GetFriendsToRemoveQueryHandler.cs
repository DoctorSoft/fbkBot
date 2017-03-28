using System.Collections.Generic;
using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Models;

namespace DataBase.QueriesAndCommands.Queries.Friends.GetFriendsToRemove
{
    public class GetFriendsToRemoveQueryHandler : IQueryHandler<GetFriendsToRemoveQuery, List<FriendData>>
    {
        private readonly DataBaseContext _context;

        public GetFriendsToRemoveQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public List<FriendData> Handle(GetFriendsToRemoveQuery query)
        {
            var account = _context.Accounts.FirstOrDefault(model => model.Id == query.AccountId);
            long groupId = 0;

            if (account != null)
            {
                if (account.GroupSettingsId != null)
                {
                    groupId = (long)account.GroupSettingsId;
                }
            }

            var result = _context.Friends
                .Where(model => !model.DeleteFromFriends)
                .Where(model => model.AddedToRemoveDateTime != null)
                .Where(model => !_context.FriendsBlackList.Any(blackListModel => blackListModel.FriendFacebookId == model.FacebookId && blackListModel.GroupId == groupId))
                .Select(model => new FriendData
                {
                    FacebookId = model.FacebookId,
                    AccountId = model.AccountId,
                    FriendName = model.FriendName,
                    Deleted = model.DeleteFromFriends,
                    Id = model.Id,
                    DialogIsCompleted = model.DialogIsCompleted,
                    MessageRegime = model.MessageRegime,
                    AddedDateTime = model.AddedDateTime,
                    Href = model.Href,
                    IsAddedToGroups = model.IsAddedToGroups,
                    IsAddedToPages = model.IsAddedToPages,
                    IsWinked = model.IsWinked,
                    Gender = model.Gender,
                    AddedToRemoveDateTime = model.AddedToRemoveDateTime,
                    IsWinkedFriendsFriend = model.IsWinkedFriendsFriend
                }).ToList();

            return result;
        }
    }
}
