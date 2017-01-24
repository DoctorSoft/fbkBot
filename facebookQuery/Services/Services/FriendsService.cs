using System;
using System.Linq;
using System.Threading;
using CommonModels;
using Constants.FriendTypesEnum;
using DataBase.Constants;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.AccountStatistics;
using DataBase.QueriesAndCommands.Commands.AnalysisFriends;
using DataBase.QueriesAndCommands.Commands.Friends.ChangeAnalysisFriendStatusCommand;
using DataBase.QueriesAndCommands.Commands.Friends.ChangeAnalysisFriendTypeCommand;
using DataBase.QueriesAndCommands.Commands.Friends.MarkRemovedFriendCommand;
using DataBase.QueriesAndCommands.Commands.Friends.RemoveAnalyzedFriendCommand;
using DataBase.QueriesAndCommands.Commands.Friends.SaveUserFriendsCommand;
using DataBase.QueriesAndCommands.Models;
using DataBase.QueriesAndCommands.Queries.Account;
using DataBase.QueriesAndCommands.Queries.AnalysisFriends;
using DataBase.QueriesAndCommands.Queries.Friends;
using DataBase.QueriesAndCommands.Queries.UrlParameters;
using Engines.Engines.CancelFriendshipRequestEngine;
using Engines.Engines.ConfirmFriendshipEngine;
using Engines.Engines.GetFriendsEngine.GetCurrentFriendsBySeleniumEngine;
using Engines.Engines.GetFriendsEngine.GetCurrentFriendsEngine;
using Engines.Engines.GetFriendsEngine.GetRecommendedFriendsEngine;
using Engines.Engines.RemoveFriendEngine;
using Engines.Engines.SendRequestFriendshipEngine;
using Services.Core.Interfaces.ServiceTools;
using Services.ServiceTools;
using Services.ViewModels.FriendsModels;
using Services.ViewModels.HomeModels;

namespace Services.Services
{
    public class FriendsService
    {
        private readonly IAccountManager _accountManager;
        private readonly IStatisticsManager _accountStatisticsManager;
        private readonly ISeleniumManager _seleniumManager;
        private readonly IAnalysisFriendsManager _analysisFriendsManager;

        public FriendsService()
        {
            _accountManager = new AccountManager();
            _accountStatisticsManager = new StatisticsManager();
            _seleniumManager = new SeleniumManager();
            _analysisFriendsManager = new AnalysisFriendsManager();
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

        public AnalizeFriendListViewModel GetIncomingRequestsFriendshipByAccount(long accountId)
        {
            var friendsIncoming = new GetFriendsByAccountIdQueryHandler(new DataBaseContext()).Handle(new GetFriendsByAccountIdQuery
            {
                AccountId = accountId,
                FriendsType = FriendTypes.Incoming
            });
            var friendsRecommended = new GetFriendsByAccountIdQueryHandler(new DataBaseContext()).Handle(new GetFriendsByAccountIdQuery
            {
                AccountId = accountId,
                FriendsType = FriendTypes.Recommended
            });

            var friends = friendsIncoming.Concat(friendsRecommended);

            var result = new AnalizeFriendListViewModel
            {
                AccountId = accountId,
                Friends = friends.Select(model => new AnalizeFriendViewModel
                {
                    FacebookId = model.FacebookId,
                    Id = model.Id,
                    AccountId = accountId,
                    FriendName = model.FriendName,
                    AddedDateTime = model.AddedDateTime,
                    Status = model.Status,
                    Type = model.Type
                }).ToList()
            };

            return result;
        }

        public AnalizeFriendListViewModel GetOutgoingRequestsFriendshipByAccount(long accountId)
        {
            var friends = new GetFriendsByAccountIdQueryHandler(new DataBaseContext()).Handle(new GetFriendsByAccountIdQuery
            {
                AccountId = accountId,
                FriendsType = FriendTypes.Outgoig
            });

            var result = new AnalizeFriendListViewModel
            {
                AccountId = accountId,
                Friends = friends.Select(model => new AnalizeFriendViewModel
                {
                    FacebookId = model.FacebookId,
                    Id = model.Id,
                    AccountId = accountId,
                    FriendName = model.FriendName,
                    AddedDateTime = model.AddedDateTime,
                    Status = model.Status
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

            var friends = new GetCurrentFriendsBySeleniumEngine().Execute(new GetCurrentFriendsBySeleniumModel
            {
                Cookie = account.Cookie.CookieString,
                AccountFacebookId = accountFacebokId,
                Driver = _seleniumManager.RegisterNewDriver(new AccountViewModel
                {
                    Proxy = account.Proxy,
                    ProxyLogin = account.ProxyLogin,
                    ProxyPassword = account.ProxyPassword
                })
            });

            var outgoingFriendships = new GetFriendsByAccountIdQueryHandler(new DataBaseContext()).Handle(new GetFriendsByAccountIdQuery
            {
                AccountId = account.Id,
                FriendsType = FriendTypes.Outgoig
            });

            foreach (var newFriend in friends)
            {
                if (outgoingFriendships.All(data => data.FacebookId != newFriend.FacebookId))
                {
                    continue;
                }

                new DeleteAnalysisFriendByIdHandler(new DataBaseContext()).Handle(new DeleteAnalysisFriendById
                {
                    AnalysisFriendFacebookId = newFriend.FacebookId
                });

                new AddOrUpdateAccountStatisticsCommandHandler(new DataBaseContext()).Handle(
                    new AddOrUpdateAccountStatisticsCommand
                    {
                        AccountId = account.Id,
                        CountOrdersConfirmedFriends = 1
                    });
            }

            if (friends.Count != 0)
            {
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
        }

        public NewFriendListViewModel GetNewFriendsAndRecommended(long accountFacebokId)
        {
            var account = new GetAccountByFacebookIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByFacebookIdQuery
            {
                FacebookUserId = accountFacebokId
            });

            var friendListResponseModels = new GetRecommendedFriendsEngine().Execute(new GetRecommendedFriendsModel()
            {
                Cookie = account.Cookie.CookieString,
                Proxy = _accountManager.GetAccountProxy(account)
            });

            var friendList = friendListResponseModels.Select(model => new AnalysisFriendData
            {
                AccountId = account.Id,
                FacebookId = model.FacebookId,
                Type = model.Type,
                Status = StatusesFriend.ToAnalys,
                FriendName = model.FriendName
            }).ToList();
            
            //Check
            var certifiedListFriends = _analysisFriendsManager.CheckForAnyInDataBase(account, friendList);

            new SaveFriendsForAnalysisCommandHandler(new DataBaseContext()).Handle(new SaveFriendsForAnalysisCommand
            {
                AccountId = account.Id,
                Friends = certifiedListFriends
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
            var account =
                new GetAccountByFacebookIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByFacebookIdQuery
                {
                    FacebookUserId = accountId
                });

            var friends = new GetFriendsToConfirmQueryHandler(new DataBaseContext()).Handle(new GetFriendsToConfirmQuery
            {
                AccountId = account.Id
            });

            if (friends.Count == 0)
            {
                return;
            }

            var analysisFriendsData = friends.FirstOrDefault();

            new ConfirmFriendshipEngine().Execute(new ConfirmFriendshipModel
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

        public void SendRequestFriendship(long accountId)
        {
            var account =
                new GetAccountByFacebookIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByFacebookIdQuery
                {
                    FacebookUserId = accountId
                });

            var friends = new GetFriendsToRequestQueryHandler(new DataBaseContext()).Handle(new GetFriendsToRequestQuery
            {
                AccountId = account.Id
            });

            if (friends.Count == 0)
            {
                return;
            }

            var analysisFriendsData = friends.FirstOrDefault();
            try
            {
                new SendRequestFriendshipEngine().Execute(new SendRequestFriendshipModel
                {
                    AccountFacebookId = account.FacebookId,
                    FriendFacebookId = analysisFriendsData.FacebookId,
                    Proxy = _accountManager.GetAccountProxy(account),
                    Cookie = account.Cookie.CookieString,
                    AddFriendUrlParameters =
                        new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
                        {
                            NameUrlParameter = NamesUrlParameter.AddFriend
                        }),
                    AddFriendExtraUrlParameters =
                        new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
                        {
                            NameUrlParameter = NamesUrlParameter.AddFriendExtra
                        }),
                });

                if (true)
                {
                    _accountStatisticsManager.UpdateAccountStatistics(new AccountStatisticsModel
                    {
                        AccountId = account.Id,
                        CountRequestsSentToFriends = 1
                    });
                }

                new ChangeAnalysisFriendTypeCommandHandler(new DataBaseContext()).Handle(new ChangeAnalysisFriendTypeCommand
                {
                    AccountId = account.Id,
                    NewType = FriendTypes.Outgoig,
                    FriendFacebookId = analysisFriendsData.FacebookId
                });
            }
            catch (Exception ex)
            {
                
            }
//            foreach (var analysisFriendsData in friends)
//            {
//                new SendRequestFriendshipEngine().Execute(new SendRequestFriendshipModel
//                {
//                    AccountFacebookId = account.FacebookId,
//                    FriendFacebookId = analysisFriendsData.FacebookId,
//                    Proxy = _accountManager.GetAccountProxy(account),
//                    Cookie = account.Cookie.CookieString,
//                    AddFriendUrlParameters = new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
//                    {
//                        NameUrlParameter = NamesUrlParameter.AddFriend
//                    }),
//                    AddFriendExtraUrlParameters = new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
//                    {
//                        NameUrlParameter = NamesUrlParameter.AddFriendExtra
//                    }),
//                });
//
//                if (true)
//                {
//                    _accountStatisticsManager.UpdateAccountStatistics(new AccountStatisticsModel
//                    {
//                        AccountId = account.Id,
//                        CountRequestsSentToFriends = 1
//                    });
//                }
//
//                new ChangeAnalysisFriendTypeCommandHandler(new DataBaseContext()).Handle(new ChangeAnalysisFriendTypeCommand
//                {
//                    AccountId = account.Id,
//                    NewType = FriendTypes.Outgoig
//                });
//
//                Thread.Sleep(2000);
//            }
        }

        public void RemoveFriend(long accountId, long friendId)
        {
            var account = new GetAccountByIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByIdQuery
            {
                UserId = accountId
            });

            var friend = new GetFriendByIdAccountQueryHandler(new DataBaseContext()).Handle(new GetFriendByIdAccountQuery
            {
                AccountId = friendId
            });

            new RemoveFriendEngine().Execute(new RemoveFriendModel
            {
                AccountFacebookId = account.FacebookId,
                Cookie = account.Cookie.CookieString,
                Proxy = _accountManager.GetAccountProxy(account),
                FriendFacebookId = friend.FacebookId,
                UrlParameters = new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
                {
                    NameUrlParameter = NamesUrlParameter.RemoveFriend
                })
            });

            if (true)
            {
                new MarkRemovedFriendCommandHandler(new DataBaseContext()).Handle(new MarkRemovedFriendCommand
                {
                    AccountId = account.Id,
                    FriendId = friend.Id
                });
            }
        }

        public void CancelFriendshipRequest(long accountId, long friendFacebookId)
        {
            var account = new GetAccountByIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByIdQuery
            {
                UserId = accountId
            });

            new CancelFriendshipRequestEngine().Execute(new CancelFriendshipRequestModel
            {
                Cookie = account.Cookie.CookieString,
                Proxy = _accountManager.GetAccountProxy(account),
                AccountFacebookId = account.FacebookId,
                FriendFacebookId = friendFacebookId,
                UrlParameters = new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
                {
                    NameUrlParameter = NamesUrlParameter.CancelRequestFriendship
                })
            });

            new ChangeAnalysisFriendStatusCommandHandler(new DataBaseContext()).Handle(new ChangeAnalysisFriendStatusCommand
            {
               AccountId = accountId,
               FriendFacebookId = friendFacebookId,
               NewStatus = StatusesFriend.ToDelete
            });
        }
    }
}
