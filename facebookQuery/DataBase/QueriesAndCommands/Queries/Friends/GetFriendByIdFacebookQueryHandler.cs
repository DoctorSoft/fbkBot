using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Models;

namespace DataBase.QueriesAndCommands.Queries.Friends
{
    public class GetFriendByIdFacebookQueryHandler : IQueryHandler<GetFriendByIdFacebookQuery, FriendData>
    {
        private readonly DataBaseContext _context;

        public GetFriendByIdFacebookQueryHandler(DataBaseContext context)
        {
            _context = context;
        }

        public FriendData Handle(GetFriendByIdFacebookQuery query)
        {
            var result = _context.Friends
                .Where(model => model.FacebookId == query.FacebookId)
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
                }).FirstOrDefault();

            return result;
        }
    }
}
