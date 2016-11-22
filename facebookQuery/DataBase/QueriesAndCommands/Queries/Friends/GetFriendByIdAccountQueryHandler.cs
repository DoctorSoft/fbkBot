using System.Collections.Generic;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.Friends
{
    public class GetFriendByIdAccountQueryHandler : IQueryHandler<GetFriendByIdAccountQuery, List<FriendData>>
    {
        private readonly DataBaseContext context;

        public GetFriendByIdAccountQueryHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public List<FriendData> Handle(GetFriendByIdAccountQuery query)
        {
            var result = context.Friends
                .Where(model => model.Id == query.FacebookId)
                .Select(model => new FriendData
                {
                    FriendId = model.FriendId,
                    FriendName = model.FriendName,
                    Deleted = model.DeleteFromFriends,
                    Id = model.Id,
                    MessagesEnded = model.IsBlocked > 0
                }).ToList();

            return result;
        }
    }
}
