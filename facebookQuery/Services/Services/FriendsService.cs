using System.Linq;
using System.Threading;
using CommonModels;
using Constants.FriendTypesEnum;
using DataBase.Constants;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.Friends.RemoveAnalyzedFriendCommand;
using DataBase.QueriesAndCommands.Commands.Friends.SaveUserFriendsCommand;
using DataBase.QueriesAndCommands.Models;
using DataBase.QueriesAndCommands.Queries.Account;
using DataBase.QueriesAndCommands.Queries.AnalysisFriends;
using DataBase.QueriesAndCommands.Queries.Friends;
using DataBase.QueriesAndCommands.Queries.UrlParameters;
using Engines.Engines.ConfirmFriendshipEngine;
using Engines.Engines.GetFriendsEngine.GetCurrentFriendsEngine;
using Engines.Engines.GetFriendsEngine.GetRecommendedFriendsEngine;
using Services.Core.Interfaces.ServiceTools;
using Services.ServiceTools;
using Services.ViewModels.FriendsModels;
using Services.ViewModels.HomeModels;

namespace Services.Services
{
    public class FriendsService
    {
        private readonly IAccountManager _accountManager;
        private readonly IAccountStatisticsManager _accountStatisticsManager;

        public FriendsService()
        {
            _accountManager = new AccountManager();
            _accountStatisticsManager = new AccountStatisticsManager();
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
                FacebookUserId = accountFacebokId
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
                    FriendName = model.FriendName,
                    Href = model.Uri,
                    Gender = model.Gender
                }).ToList()
            });
        }

        public NewFriendListViewModel GetNewFriendsAndRecommended(long accountFacebokId)
        {
            var account = new GetAccountByFacebookIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByFacebookIdQuery
            {
                FacebookUserId = accountFacebokId
            });

            var friendList = new GetRecommendedFriendsEngine().Execute(new GetRecommendedFriendsModel()
            {
                Cookie = account.Cookie.CookieString,
                Proxy = _accountManager.GetAccountProxy(account)
            });

            new SaveFriendsForAnalysisCommandHandler(new DataBaseContext()).Handle(new SaveFriendsForAnalysisCommand
            {
                AccountId = account.Id,
                Friends = friendList.Select(model => new AnalysisFriendData
                {
                    AccountId = account.Id,
                    FacebookId = model.FacebookId,
                    Type = model.Type,
                    Status = StatusesFriend.ToAnalys,
                    FriendName = model.FriendName
                }).ToList()
            });

            return new NewFriendListViewModel
            {
                AccountId = account.Id,
                NewFriends = friendList.Select(model => new NewFriendViewModel
                {
                    FacebookId = model.FacebookId,
                    FriendName = model.FriendName,
                    Gender = model.Gender,
                    Type = model.Type,
                    Uri = model.Uri
                }).ToList()
            };
        }

        public void ConfirmFriendship(long accountId)
        {
            var account = new GetAccountByFacebookIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByFacebookIdQuery
            {
                FacebookUserId = accountId
            });

            var friends = new GetFriendsToConfirmQueryHandler(new DataBaseContext()).Handle(new GetFriendsToConfirmQuery
            {
                AccountId = account.Id
            });

            foreach (var analysisFriendsData in friends)
            {
                new ConfirmFriendshipEngine().Execute(new ConfirmFriendshipModel()
                {
                    AccountFacebookId = account.FacebookId,
                    FriendFacebookId = analysisFriendsData.FacebookId,
                    Proxy = _accountManager.GetAccountProxy(account),
                    Cookie = account.Cookie.CookieString,
                    UrlParameters = new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
                    {
                        NameUrlParameter = NamesUrlParameter.ConfirmFriendship
                    }),
                });

                if (true)
                {
                    _accountStatisticsManager.UpdateAccountStatistics(new AccountStatisticsModel
                    {
                        AccountId = account.Id,
                        CountReceivedFriends = 1
                    });
                }

                new RemoveAnalyzedFriendCommandHandler(new DataBaseContext()).Handle(new RemoveAnalyzedFriendCommand
                {
                    AccountId = account.Id,
                    FriendId = analysisFriendsData.Id
                });

                Thread.Sleep(2000);
            }

        }
    }
}
