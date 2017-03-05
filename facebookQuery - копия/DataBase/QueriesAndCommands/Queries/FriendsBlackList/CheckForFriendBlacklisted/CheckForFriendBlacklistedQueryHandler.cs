using System.Linq;
using System.Security.Cryptography.X509Certificates;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.FriendsBlackList.CheckForFriendBlacklisted
{
    public class CheckForFriendBlacklistedQueryHandler : IQueryHandler<CheckForFriendBlacklistedQuery, bool>
    {
        private readonly DataBaseContext context;

        public CheckForFriendBlacklistedQueryHandler()
        {
            context = new DataBaseContext();
        }
        public bool Handle(CheckForFriendBlacklistedQuery query)
        {
            var friendDbModel =
                context.FriendsBlackList.FirstOrDefault(model => model.FriendFacebookId == query.FriendFacebookId && model.GroupId == query.GroupSettingsId);

            return friendDbModel != null;
        }
    }
}
