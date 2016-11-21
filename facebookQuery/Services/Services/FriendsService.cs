using System.Linq;
using CommonModels;
using DataBase.Constants;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.Friends.SaveUserFriendsCommand;
using DataBase.QueriesAndCommands.Queries.Account;
using DataBase.QueriesAndCommands.Queries.Friends;
using DataBase.QueriesAndCommands.Queries.UrlParameters;
using Engines.Engines.GetFriendsEngine;
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
                    Name = model.FriendName,
                    Deleted = model.Deleted,
                    Id = model.Id,
                    MessagesEnded = model.MessagesEnded
                }).ToList()
            };

            return result;
        }

        public void GetFriendsOfFacebook(long accountId)
        {
            var account = new GetAccountByIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByIdQuery
            {
                UserId = accountId
            });

            var urlParameters = new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
            {
                NameUrlParameter = NamesUrlParameter.GetFriends
            });

            var friends = new GetFriendsEngine().Execute(new GetFriendsModel()
            {
                Cookie = account.Cookie.CookieString,
                AccountId = accountId,
                UrlParameters = urlParameters
            });

            SaveFriends(new FriendListViewModel
            {
                AccountId = account.Id,
                Friends = friends.Select(model => new FriendViewModel
                {
                    FriendId = model.FriendId,
                    Name = model.FriendName,
                }).ToList()
            });
        }

        public void SaveFriends(FriendListViewModel friendListViewModel)
        {

            new SaveUserFriendsCommandHandler(new DataBaseContext()).Handle(new SaveUserFriendsCommand()
            {
                AccountId = friendListViewModel.AccountId,
                Friends = friendListViewModel.Friends.Select(model => new GetFriendsResponseModel()
                {
                    FriendId = model.FriendId,
                    FriendName = model.Name
                }).ToList()
            });
        }
    }
}
