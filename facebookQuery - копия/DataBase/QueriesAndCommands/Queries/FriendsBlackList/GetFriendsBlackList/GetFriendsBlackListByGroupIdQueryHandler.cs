using System.Collections.Generic;
using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Models;

namespace DataBase.QueriesAndCommands.Queries.FriendsBlackList.GetFriendsBlackList
{
    public class GetFriendsBlackListByGroupIdQueryHandler : IQueryHandler<GetFriendsBlackListByGroupIdQuery, List<BlockedFriendData>>
    {
        private readonly DataBaseContext context;

        public GetFriendsBlackListByGroupIdQueryHandler(DataBaseContext context)
        {
            this.context = context;
        }
        public List<BlockedFriendData> Handle(GetFriendsBlackListByGroupIdQuery query)
        {
            var results = context.FriendsBlackList
                .Where(model => model.GroupId == query.GroupSettingsId)
                .OrderByDescending(model => model.DateAdded)
                .Select(model => new BlockedFriendData
                {
                    Id = model.Id,
                    FacebookId = model.FriendFacebookId,
                    FriendName = model.FriendName,
                    GroupId = model.GroupId,
                    DateAdded = model.DateAdded
                }).ToList();

            return results;
        }
    }
}
