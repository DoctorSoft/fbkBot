using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.FriendsBlackList.CheckForFriendBlacklisted
{
    public class CheckForFriendBlacklistedQueryHandler : IQueryHandler<CheckForFriendBlacklistedQuery, bool>
    {
        private readonly DataBaseContext _context;

        public CheckForFriendBlacklistedQueryHandler()
        {
            _context = new DataBaseContext();
        }
        public bool Handle(CheckForFriendBlacklistedQuery query)
        {
            var friendDbModel =
                _context.FriendsBlackList.FirstOrDefault(model => model.FriendFacebookId == query.FriendFacebookId && model.GroupId == query.GroupSettingsId);

            return friendDbModel != null;
        }
    }
}
