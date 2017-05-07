using System;
using System.Collections.Generic;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.Friends.GetFriendsToWinkFriendsFriends
{
    public class GetFriendsToWinkFriendsFriendsQueryHandler : IQueryHandler<GetFriendsToWinkFriendsFriendsQuery, List<GetFriendsToWinkFriendsFriendsResponseModel>>
    {
        private readonly DataBaseContext _context;

        public GetFriendsToWinkFriendsFriendsQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public List<GetFriendsToWinkFriendsFriendsResponseModel> Handle(GetFriendsToWinkFriendsFriendsQuery query)
        {
            var friends = _context.Friends
                .Where(model => model.AccountId == query.AccountId)
                .Where(model => model.AddedToRemoveDateTime == null)//не добавлен к удалению
                .Where(model => !model.IsWinkedFriendsFriend)//не подмигивали
                .Where(model => !_context.FriendsBlackList.Any(dbModel => dbModel.FriendFacebookId == model.FacebookId && dbModel.GroupId == query.GroupSettingsId))//не в чс
                .Select(model => new GetFriendsToWinkFriendsFriendsResponseModel
                {
                    FriendFacebookId = model.FacebookId,
                    FriendId = model.Id
                });

            if (query.CountFriends != 0)
            {
                friends = friends.OrderBy(x => Guid.NewGuid()).Take(query.CountFriends);
            }
            else
            {
                friends = friends.Take(5);
            }

            return friends.ToList();
        }
    }
}
