using System.Linq;
using System.Runtime.InteropServices;
using CommonModels;
using DataBase.Constants;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.Friends.SaveUserFriendsCommand;
using DataBase.QueriesAndCommands.Queries.Account;
using DataBase.QueriesAndCommands.Queries.Friends;
using DataBase.QueriesAndCommands.Queries.UrlParameters;
using Engines.Engines.GetFriendsEngine;
using Services.Core.Interfaces.ServiceTools;
using Services.ServiceTools;
using Services.ViewModels.FriendsModels;

namespace Services.Services
{
    public class FriendsService
    {
        private readonly IAccountManager _accountManager;

        public FriendsService()
        {
            _accountManager = new AccountManager();
        }

        public FriendListViewModel GetFriendsByAccount(long accountFacebokId)
        {
            var friends = new GetFriendsByAccountQueryHandler(new DataBaseContext()).Handle(new GetFriendsByAccountQuery
            {
                AccountId = accountFacebokId
            });

            var result = new FriendListViewModel
            {
                AccountId = accountFacebokId,
                Friends = friends.Select(model => new FriendViewModel
                {
                    FacebookId = model.FacebookId,
                    Name = model.FriendName,
                    Deleted = model.Deleted,
                    Id = model.Id,
                    MessagesEnded = model.MessagesEnded
                }).ToList()
            };

            return result;
        }

        public void GetFriendsOfFacebook(long accountFacebokId)
        {
            var account = new GetAccountByFacebookIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByFacebookIdQuery
            {
                UserId = accountFacebokId
            });

            var urlParameters = new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
            {
                NameUrlParameter = NamesUrlParameter.GetFriends
            });

            var friends = new GetFriendsEngine().Execute(new GetFriendsModel()
            {
                Cookie = account.Cookie.CookieString,
                AccountId = accountFacebokId,
                UrlParameters = urlParameters,
                Proxy = _accountManager.GetAccountProxy(account)
            });
                
            new SaveUserFriendsCommandHandler(new DataBaseContext()).Handle(new SaveUserFriendsCommand()
            {
                AccountId = account.Id,
                Friends = friends.Select(model => new FriendData()
                {
                    FacebookId = model.FacebookId,
                    FriendName = model.FriendName
                }).ToList()
            });
        }
    }
}
