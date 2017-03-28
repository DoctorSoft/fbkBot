using System.Collections.Generic;
using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Models;

namespace DataBase.QueriesAndCommands.Queries.Friends
{
    public class GetFriendsByAccountQueryHandler : IQueryHandler<GetFriendsByAccountQuery, List<FriendData>>
    {
        private readonly DataBaseContext _context;

        public GetFriendsByAccountQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public List<FriendData> Handle(GetFriendsByAccountQuery query)
        {
            var result = _context.Friends
                .Where(model => model.AccountId == query.AccountId)
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
