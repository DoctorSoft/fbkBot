using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Models;

namespace DataBase.QueriesAndCommands.Queries.Friends
{
    public class GetFriendByIdAccountQueryHandler : IQueryHandler<GetFriendByIdAccountQuery, FriendData>
    {
        private readonly DataBaseContext _context;

        public GetFriendByIdAccountQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public FriendData Handle(GetFriendByIdAccountQuery query)
        {
            var result = _context.Friends
                .Where(model => model.Id == query.AccountId)
                .Select(model => new FriendData
                {
                    FacebookId = model.FacebookId,
                    AccountId = model.AccountId,
                    FriendName = model.FriendName,
                    Deleted = model.DeleteFromFriends,
                    Id = model.Id,
                    DialogIsCompleted = model.DialogIsCompleted,
                    MessageRegime = model.MessageRegime
                }).FirstOrDefault();

            return result;
        }
    }
}
