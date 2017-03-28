using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Models;

namespace DataBase.QueriesAndCommands.Queries.Friends.GetFriendById
{
    public class GetFriendByIdQueryHandler : IQueryHandler<GetFriendByIdQuery, FriendData>
    {
        private readonly DataBaseContext _context;

        public GetFriendByIdQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public FriendData Handle(GetFriendByIdQuery query)
        {
            var result = _context.Friends
                .Where(model => model.AccountId == query.FriendId)
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
                    Gender = model.Gender,
                    Href = model.Href,
                    IsAddedToGroups = model.IsAddedToGroups,
                    IsAddedToPages = model.IsAddedToPages,
                    IsWinked = model.IsWinked,
                    AddedToRemoveDateTime = model.AddedToRemoveDateTime,
                    IsWinkedFriendsFriend = model.IsWinkedFriendsFriend
                }).FirstOrDefault();

            return result;
        }
    }
}
