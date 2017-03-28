using System.Collections.Generic;
using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Models;

namespace DataBase.QueriesAndCommands.Queries.Friends.GetFriendsToQueueDeletion
{
    public class GetFriendsToQueueDeletionQueryHandler : IQueryHandler<GetFriendsToQueueDeletionQuery, List<FriendData>>
    {
        private readonly DataBaseContext _context;

        public GetFriendsToQueueDeletionQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public List<FriendData> Handle(GetFriendsToQueueDeletionQuery query)
        {
            var result = _context.Friends
                .Where(model => !model.DialogIsCompleted || !model.IsAddedToGroups || !model.IsAddedToPages || !model.IsWinked)
                .Where(model => model.AddedToRemoveDateTime == null) // не помеен для удаления
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
