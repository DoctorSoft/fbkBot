using DataBase.Context;
using DataBase.QueriesAndCommands.Queries.Friends;
using Services.Core.Interfaces.ServiceTools;

namespace Services.ServiceTools
{
    public class FriendManager : IFriendManager
    {
        public FriendData GetFriendByFacebookId(long friendFacebookId)
        {
            return new GetFriendByIdFacebookQueryHandler(new DataBaseContext()).Handle(new GetFriendByIdFacebookQuery
            {
                FacebookId = friendFacebookId
            });
        }

        public FriendData GetFriendById(long friendAccountId)
        {
            return new GetFriendByIdAccountQueryHandler(new DataBaseContext()).Handle(new GetFriendByIdAccountQuery
            {
                AccountId = friendAccountId
            });
        }
    }
}
