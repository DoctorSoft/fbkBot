using System.Collections.Generic;
using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Models;

namespace DataBase.QueriesAndCommands.Queries.Friends
{
    public class GetFriendByIdFacebookQueryHandler : IQueryHandler<GetFriendByIdFacebookQuery, FriendData>
    {
        private readonly DataBaseContext context;

        public GetFriendByIdFacebookQueryHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public FriendData Handle(GetFriendByIdFacebookQuery query)
        {
            var result = context.Friends
                .Where(model => model.FacebookId == query.FacebookId)
                .Select(model => new FriendData
                {
                    FacebookId = model.FacebookId,
                    AccountId = model.AccountId,
                    FriendName = model.FriendName,
                    Deleted = model.DeleteFromFriends,
                    Id = model.Id,
                    MessagesEnded = model.IsBlocked,
                    MessageRegime = model.MessageRegime
                }).FirstOrDefault();

            return result;
        }
    }
}
