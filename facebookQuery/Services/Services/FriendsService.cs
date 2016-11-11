using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Queries.Friends;
using Services.ViewModels.FriendsModels;

namespace Services.Services
{
    public class FriendsService
    {
        public FriendListViewModel GetFriendsByAccount(long accountId)
        {
            var friends = new GetFriendsByAccountQueryHandler(new DataBaseContext()).Handle(new GetFriendsByAccountQuery
            {
                AccountId = accountId
            });

            var result = new FriendListViewModel
            {
                AccountId = accountId,
                Friends = friends.Select(model => new FriendViewModel
                {
                    FriendId = model.FriendId,
                    Name = model.FriendName
                }).ToList()
            };

            return result;
        }
    }
}
