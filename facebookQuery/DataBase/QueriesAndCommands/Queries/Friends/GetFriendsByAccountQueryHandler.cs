using System.Collections.Generic;
using System.Linq;
using CommonModels;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.Friends
{
    public class GetFriendsByAccountQueryHandler : IQueryHandler<GetFriendsByAccountQuery, List<GetFriendsResponseModel>>
    {
        private readonly DataBaseContext context;

        public GetFriendsByAccountQueryHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public List<GetFriendsResponseModel> Handle(GetFriendsByAccountQuery query)
        {
            var result = context.Friends
                .Where(model => model.AccountId == query.AccountId)
                .Select(model => new GetFriendsResponseModel
                {
                    FriendId = model.FriendId,
                    FriendName = model.FriendName
                }).ToList();

            return result;
        }
    }
}
