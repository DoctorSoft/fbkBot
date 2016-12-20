using System.Collections.Generic;
using System.Linq;
using CommonModels;
using DataBase.Context;
using DataBase.QueriesAndCommands.Models;

namespace DataBase.QueriesAndCommands.Queries.Friends
{
    public class GetFriendsByAccountQueryHandler : IQueryHandler<GetFriendsByAccountQuery, List<FriendData>>
    {
        private readonly DataBaseContext context;

        public GetFriendsByAccountQueryHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public List<FriendData> Handle(GetFriendsByAccountQuery query)
        {
            var result = context.Friends
                .Where(model => model.AccountId == query.AccountId)
                .Select(model => new FriendData
                {
                    FacebookId = model.FacebookId,
                    AccountId = model.AccountId,
                    FriendName = model.FriendName,
                    Deleted = model.DeleteFromFriends,
                    Id = model.Id,
                    MessagesEnded = model.IsBlocked
                }).ToList();

            return result;
        }
    }
}
