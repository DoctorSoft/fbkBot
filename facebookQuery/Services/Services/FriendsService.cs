using System;
using System.Linq;
using System.Threading;
using CommonInterfaces.Interfaces.Services;
using CommonModels;
using Constants.FriendTypesEnum;
using DataBase.Constants;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.AccountInformation;
using DataBase.QueriesAndCommands.Commands.AccountStatistics;
using DataBase.QueriesAndCommands.Commands.AnalysisFriends;
using DataBase.QueriesAndCommands.Commands.Friends.ChangeAnalysisFriendStatusCommand;
using DataBase.QueriesAndCommands.Commands.Friends.ChangeAnalysisFriendTypeCommand;
using DataBase.QueriesAndCommands.Commands.Friends.MarkAddToEndDialogCommand;
using DataBase.QueriesAndCommands.Commands.Friends.MarkAddToGroupFriendCommand;
using DataBase.QueriesAndCommands.Commands.Friends.MarkAddToPageFriendCommand;
using DataBase.QueriesAndCommands.Commands.Friends.MarkAddToRemovedFriendCommand;
using DataBase.QueriesAndCommands.Commands.Friends.MarkRemovedFriendCommand;
using DataBase.QueriesAndCommands.Commands.Friends.MarkWinkedFriendCommand;
using DataBase.QueriesAndCommands.Commands.Friends.MarkWinkedFriendsFriendCommand;
using DataBase.QueriesAndCommands.Commands.Friends.RemoveAnalyzedFriendCommand;
using DataBase.QueriesAndCommands.Commands.Friends.SaveUserFriendsCommand;
using DataBase.QueriesAndCommands.Models;
using DataBase.QueriesAndCommands.Models.JsonModels.AccountInformationModels;
using DataBase.QueriesAndCommands.Queries.Account;
using DataBase.QueriesAndCommands.Queries.Account.Models;
using DataBase.QueriesAndCommands.Queries.AnalysisFriends;
using DataBase.QueriesAndCommands.Queries.Friends;
using DataBase.QueriesAndCommands.Queries.Friends.GetFriendById;
using DataBase.QueriesAndCommands.Queries.Friends.GetFriendsToQueueDeletion;
using DataBase.QueriesAndCommands.Queries.Friends.GetFriendsToRemove;
using DataBase.QueriesAndCommands.Queries.Friends.GetFriendsToWink;
using DataBase.QueriesAndCommands.Queries.FriendsBlackList.CheckForFriendBlacklisted;
using DataBase.QueriesAndCommands.Queries.UrlParameters;
using Engines.Engines.CancelFriendshipRequestEngine;
using Engines.Engines.ConfirmFriendshipEngine;
using Engines.Engines.GetFriendsCountEngine;
using Engines.Engines.GetFriendsEngine.GetCurrentFriendsBySeleniumEngine;
using Engines.Engines.GetFriendsEngine.GetRecommendedFriendsEngine;
using Engines.Engines.RemoveFriendEngine;
using Engines.Engines.SendRequestFriendshipEngine;
using Engines.Engines.WinkEngine;
using Services.Interfaces.Notices;
using Services.Interfaces.ServiceTools;
using Services.ServiceTools;
using Services.ViewModels.FriendsModels;
using Services.ViewModels.HomeModels;

namespace Services.Services
{
    public class FriendsService
    {
        private readonly INoticesProxy _notice;
        private readonly IAccountManager _accountManager;
        private readonly IFriendManager _friendManager;
        private readonly IAccountSettingsManager _accountSettingsManager;
        private readonly IStatisticsManager _accountStatisticsManager;
        private readonly ISeleniumManager _seleniumManager;
        private readonly IAnalysisFriendsManager _analysisFriendsManager;

        public FriendsService(INoticesProxy noticeProxy)
        {
            _notice = noticeProxy;
            _accountManager = new AccountManager();
            _friendManager = new FriendManager();
            _accountSettingsManager = new AccountSettingsManager();
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
                    AddDateTime = model.AddedDateTime,
                    Id = model.Id,
                    Deleted = model.Deleted,
                    FacebookId = model.FacebookId,
                    MessagesEnded = model.DialogIsCompleted,
                    Name = model.FriendName,
                    Href = model.Href,
                    IsAddedToGroups = model.IsAddedToGroups,
                    IsAddedToPages = model.IsAddedToPages,
                    IsWinked = model.IsWinked,
                    MessageRegime = model.MessageRegime,
                    AddedToRemoveDateTime = model.AddedToRemoveDateTime
                }).ToList()
            };

            return result;
        }

        public FriendViewModel GetFriendById(long friendId)
        {
            var friend = new GetFriendByIdQueryHandler(new DataBaseContext()).Handle(new GetFriendByIdQuery
            {
                FriendId = friendId
            });

            return new FriendViewModel
            {
                AddDateTime = friend.AddedDateTime,
                Id = friend.Id,
                Deleted = friend.Deleted,
                FacebookId = friend.FacebookId,
                MessagesEnded = friend.DialogIsCompleted,
                Name = friend.FriendName,
                Href = friend.Href,
                IsAddedToGroups = friend.IsAddedToGroups,
                IsAddedToPages = friend.IsAddedToPages,
                IsWinked = friend.IsWinked,
                MessageRegime = friend.MessageRegime,
                AddedToRemoveDateTime = friend.AddedToRemoveDateTime
            };
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

        public bool GetCurrentFriends(AccountViewModel account)
        {
            _notice.AddNotice(account.Id, "Начинаем обновлять список друзей");
            
            var groupId = account.GroupSettingsId;

            if (groupId == null)
            {
                _notice.AddNotice(account.Id, "Ошибка! Группа не выбрана.");
                return false;
            }

            var settings = _accountSettingsManager.GetSettings((long)groupId);
            var accountInformation = _accountManager.GetAccountInformation((long)groupId);
 
            var accountDbModel = _accountManager.GetAccountById(account.Id); //for task in optionController

            var accountModel = new AccountModel
            {
                AuthorizationDataIsFailed = accountDbModel.AuthorizationDataIsFailed,
                Cookie = new CookieModel
                {
                    CookieString = accountDbModel.Cookie.CookieString,
                    CreateDateTime = accountDbModel.Cookie.CreateDateTime
                },
                ProxyDataIsFailed = accountDbModel.ProxyDataIsFailed,
                Id = accountDbModel.Id,
                Name = accountDbModel.Name,
                Proxy = accountDbModel.Proxy,
                FacebookId = accountDbModel.FacebookId,
                GroupSettingsId = accountDbModel.GroupSettingsId,
                Login = accountDbModel.Login,
                PageUrl = accountDbModel.PageUrl,
                Password = accountDbModel.Password,
                ProxyLogin = accountDbModel.ProxyLogin,
                ProxyPassword = accountDbModel.ProxyPassword
            };

            _notice.AddNotice(account.Id, "Получаем текущих друзей");

            var friendsModels = new GetCurrentFriendsBySeleniumEngine().Execute(new GetCurrentFriendsBySeleniumModel
            {
                Cookie = accountModel.Cookie.CookieString,
                AccountFacebookId = accountModel.FacebookId,
                Driver = _seleniumManager.RegisterNewDriver(new AccountViewModel
                {
                    Proxy = accountModel.Proxy,
                    ProxyLogin = accountModel.ProxyLogin,
                    ProxyPassword = accountModel.ProxyPassword
                })
            });

            var friends = friendsModels.Friends;

            _notice.AddNotice(account.Id, "Список текущих друзей получен, количество - " + friends.Count);

            _notice.AddNotice(account.Id, string.Format("Учитываем погрешность {0}%", settings.AllowedRemovalPercentage));

            var notError = _friendManager.RecountError(accountInformation.CountCurrentFriends, friends.Count,
                settings.AllowedRemovalPercentage);

            if (!notError)
            {
                _notice.AddNotice(account.Id, string.Format("Ошибка получения друзей. Получено - {0}, текущее колиство в базе - {1}, погрешность - {2}%", friends.Count, settings.AllowedRemovalPercentage, settings.AllowedRemovalPercentage));
                return false;
            }

            var outgoingFriendships = new GetFriendsByAccountIdQueryHandler(new DataBaseContext()).Handle(new GetFriendsByAccountIdQuery
            {
                AccountId = accountModel.Id,
                FriendsType = FriendTypes.Outgoig
            });


            if (friends.Count == 0)
            {
                _notice.AddNotice(account.Id, "Нет друзей. Обновление друзей завершено.");
                return true;
            }

            _notice.AddNotice(account.Id, "Сверяем друзей с исходящими заявками");
            foreach (var newFriend in friends)
            {
                if (outgoingFriendships.All(data => data.FacebookId != newFriend.FacebookId))
                {
                    continue;
                }

                _notice.AddNotice(account.Id, newFriend.FriendName + "Добавился в друзья ");

                new DeleteAnalysisFriendByIdHandler(new DataBaseContext()).Handle(new DeleteAnalysisFriendById
                {
                    AnalysisFriendFacebookId = newFriend.FacebookId
                });

                new AddOrUpdateAccountStatisticsCommandHandler(new DataBaseContext()).Handle(
                    new AddOrUpdateAccountStatisticsCommand
                    {
                        AccountId = accountModel.Id,
                        CountOrdersConfirmedFriends = 1
                    });

                _notice.AddNotice(account.Id, "Сверяем друзей с исходящими заявками");
            }

            // drop blocked users
            _notice.AddNotice(account.Id, "Сверяем друзей с черным списком");

            var newFriendList = (from friend in friends
                let isBlocked = new CheckForFriendBlacklistedQueryHandler().Handle(new CheckForFriendBlacklistedQuery
                {
                    FriendFacebookId = friend.FacebookId,
                    GroupSettingsId = (long)groupId
                })
                where !isBlocked
                select new FriendData
                {
                    FacebookId = friend.FacebookId, 
                    FriendName = friend.FriendName, 
                    Href = friend.Uri, 
                    Gender = friend.Gender
                }).ToList();

            _notice.AddNotice(account.Id, "Совпадений с черным списком - " + (friends.Count - newFriendList.Count));

            _notice.AddNotice(account.Id, "Сохраняем друзей");

            new SaveUserFriendsCommandHandler(new DataBaseContext()).Handle(new SaveUserFriendsCommand
            {
                AccountId = accountModel.Id,
                Friends = newFriendList
            });

            new AddAccountInformationCommandHandler(new DataBaseContext()).Handle(new AddAccountInformationCommand
            {
                AccountId = account.Id,
                AccountInformationData = new AccountInformationDataDbModel
                {
                    CountCurrentFriends = friends.Count
                }
            });

            _notice.AddNotice(account.Id, "Обновление друзей завершено.");
            return true;
        }

        public FriendViewModel GetFriendToWink(AccountViewModel account)
        {
            var groupId = account.GroupSettingsId;

            if (groupId == null)
            {
                return null;
            }

            var model = new GetFriendToWinkQueryHandler(new DataBaseContext()).Handle(new GetFriendToWinkQuery
            {
                AccountId = account.Id,
                GroupSettingsId = (long)groupId
            });

            if (model == null)
            {
                return null;    
            }

            return new FriendViewModel
            {
                AddDateTime = model.AddedDateTime,
                Id = model.Id,
                Deleted = model.Deleted,
                FacebookId = model.FacebookId,
                MessagesEnded = model.DialogIsCompleted,
                Name = model.FriendName,
                Href = model.Href,
                IsAddedToGroups = model.IsAddedToGroups,
                IsAddedToPages = model.IsAddedToPages,
                IsWinked = model.IsWinked,
                MessageRegime = model.MessageRegime,
                AddedToRemoveDateTime = model.AddedToRemoveDateTime
            };
        }

        public void WinkFriend(AccountViewModel account, long friendId)
        {
            if (account.GroupSettingsId == null)
            {
                return;
            }

            var friend = _friendManager.GetFriendById(friendId);

            if (friend == null)
            {
                _notice.AddNotice(account.Id, string.Format("Нет друзей для подмигивания"));
                return;
            }

            _notice.AddNotice(account.Id, string.Format("Подмигиваем другу {0}({1})", friend.FriendName, friend.FacebookId));
            new WinkEngine().Execute(new WinkModel
            {
                AccountFacebookId = account.FacebookId,
                Proxy = _accountManager.GetAccountProxy(new AccountModel
                {
                    Proxy = account.Proxy,
                    ProxyLogin = account.ProxyLogin,
                    ProxyPassword = account.ProxyPassword
                }),
                Cookie = account.Cookie,
                FriendFacebookId = friend.FacebookId,
                UrlParameters = new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
                {
                    NameUrlParameter = NamesUrlParameter.Wink
                })
            });

            _notice.AddNotice(account.Id, string.Format("Делаем отметку о подмигивании другу {0}({1})", friend.FriendName, friend.FacebookId));
            new MarkWinkedFriendCommandHandler(new DataBaseContext()).Handle(new MarkWinkedFriendCommand
            {
                FriendId = friendId,
                AccountId = account.Id
            });
        }

        public void RemoveFriends(AccountViewModel account)
        {
            if (account.GroupSettingsId == null)
            {
                return;
            }

            var settings = _accountSettingsManager.GetSettings((long) account.GroupSettingsId);
            var removeTimer = settings.DeletionFriendTimer;
            var minFriends = settings.CountMinFriends;

            _notice.AddNotice(account.Id, string.Format("Получаем друзей для удаления"));

            var friendsToRemove = new GetFriendsToRemoveQueryHandler(new DataBaseContext()).Handle(
            new GetFriendsToRemoveQuery
            {
                AccountId = account.Id
            });

            _notice.AddNotice(account.Id, string.Format("Получено {0} друзей", friendsToRemove.Count));

            foreach (var friendData in friendsToRemove)
            {
                var isReadyToRemove = friendData.AddedToRemoveDateTime != null && _friendManager.CheckConditionTime((DateTime)friendData.AddedToRemoveDateTime, removeTimer);

                _notice.AddNotice(account.Id, string.Format("Статус для удаления друга {0}({1}) = {2}", friendData.FriendName, friendData.FacebookId, isReadyToRemove));

                if (!isReadyToRemove)
                {
                    continue;
                }

                var currentFriendsCount = new GetFriendsCountEngine().Execute(new GetFriendsCountModel
                {
                    AccountFacebookId = account.FacebookId,
                    Cookie = account.Cookie,
                    Proxy = _accountManager.GetAccountProxy(new AccountModel
                    {
                        Proxy = account.Proxy,
                        ProxyLogin = account.ProxyLogin,
                        ProxyPassword = account.ProxyPassword
                    })
                });

                if (currentFriendsCount > minFriends)
                {
                    RemoveFriend(account.Id, friendData.Id);
                }
                else
                {
                    _notice.AddNotice(account.Id, string.Format("Достигнут минимальный предел для удаления из друзей ({0})", minFriends));
                }
            }
        }
        
        public void CheckFriendsAtTheEndTimeConditions(AccountViewModel account)
        {
            if (account.GroupSettingsId == null)
            {
                return;
            }

            var settings = _accountSettingsManager.GetSettings((long) account.GroupSettingsId);

            var friendsToCheck = new GetFriendsToQueueDeletionQueryHandler(new DataBaseContext()).Handle(
                new GetFriendsToQueueDeletionQuery
                {
                    AccountId = account.Id
                });

            _notice.AddNotice(account.Id, string.Format("Проверяем выполнение условий для друзей (отметка выполнения условий)... В очереди {0} друзей...", friendsToCheck.Count));

            foreach (var friend in friendsToCheck)
            {
                var readyToRemove = true;

                _notice.AddNotice(account.Id, string.Format("Проверяем {0}", friend.FriendName));

                if (settings.EnableDialogIsOver)
                {
                    var settingsTime = settings.DialogIsOverTimer;
                    var itsTime = _friendManager.CheckConditionTime(friend.AddedDateTime, settingsTime);
                    if (itsTime)
                    {
                        new MarkAddToEndDialogCommandHandler(new DataBaseContext()).Handle(
                            new MarkAddToEndDialogCommand
                            {
                                FriendId = friend.Id,
                                AccountId = account.Id
                            });
                    }
                    else
                    {
                        readyToRemove = false;
                    }
                }

                if (settings.EnableIsAddedToGroupsAndPages)
                {
                    var settingsTime = settings.IsAddedToGroupsAndPagesTimer;
                    var itsTime = _friendManager.CheckConditionTime(friend.AddedDateTime, settingsTime);
                    if (itsTime)
                    {
                        new MarkAddToGroupFriendCommandHandler(new DataBaseContext()).Handle(
                            new MarkAddToGroupFriendCommand
                            {
                                FriendId = friend.Id,
                                AccountId = account.Id
                            });

                        new MarkAddToPageFriendCommandHandler(new DataBaseContext()).Handle(
                            new MarkAddToPageFriendCommand
                            {
                                FriendId = friend.Id,
                                AccountId = account.Id
                            });
                    }
                    else
                    {
                        readyToRemove = false;
                    }
                }
                if (settings.EnableIsWink)
                {
                    var settingsTime = settings.IsWinkTimer;
                    var itsTime = _friendManager.CheckConditionTime(friend.AddedDateTime, settingsTime);
                    if (itsTime)
                    {
                        new MarkWinkedFriendCommandHandler(new DataBaseContext()).Handle(
                            new MarkWinkedFriendCommand
                            {
                                FriendId = friend.Id,
                                AccountId = account.Id
                            });
                    }
                    else
                    {
                        readyToRemove = false;
                    }
                }
                if (settings.EnableIsWinkFriendsOfFriends)
                {
                    var settingsTime = settings.IsWinkFriendsOfFriendsTimer;
                    var itsTime = _friendManager.CheckConditionTime(friend.AddedDateTime, settingsTime);
                    if (itsTime)
                    {
                        new MarkWinkedFriendsFriendCommandHandler(new DataBaseContext()).Handle(
                            new MarkWinkedFriendsFriendCommand
                            {
                                FriendId = friend.Id,
                                AccountId = account.Id
                            });
                    }
                    else
                    {
                        readyToRemove = false;
                    }
                }

                if (readyToRemove)
                {
                    //не стоит ни 1 галки
                    if (!settings.EnableDialogIsOver && !settings.EnableIsAddedToGroupsAndPages &&
                        !settings.EnableIsWink && !settings.EnableIsWinkFriendsOfFriends)
                    {
                        return;
                    }

                    // помечаем время отсчета для удаления из друзей
                    new MarkAddToRemovedFriendCommandHandler(new DataBaseContext()).Handle(new MarkAddToRemovedFriendCommand
                    {
                        FriendId = friend.Id,
                        AccountId = account.Id
                    });
                }
            }
        }

        public NewFriendListViewModel GetNewFriendsAndRecommended(AccountViewModel account, IBackgroundJobService backgroundJobService)
        {
            if (account.GroupSettingsId == null)
            {
                _notice.AddNotice(account.Id, "Ошибка! Не выбрана группа настрект.");
                return null;
            }

            var accountModel = new AccountModel
            {
                AuthorizationDataIsFailed = account.AuthorizationDataIsFailed,
                Cookie = new CookieModel
                {
                    CookieString = account.Cookie
                },
                ProxyDataIsFailed = account.ProxyDataIsFailed,
                Id = account.Id,
                Name = account.Name,
                Proxy = account.Proxy,
                FacebookId = account.FacebookId,
                GroupSettingsId = account.GroupSettingsId,
                Login = account.Login,
                PageUrl = account.PageUrl,
                Password = account.Password,
                ProxyLogin = account.ProxyLogin,
                ProxyPassword = account.ProxyPassword
            };

            _notice.AddNotice(account.Id, "Получаем рекомендованных друзей");

            var friendListResponseModels = new GetRecommendedFriendsEngine().Execute(new GetRecommendedFriendsModel
            {
                Cookie = accountModel.Cookie.CookieString,
                Proxy = _accountManager.GetAccountProxy(accountModel)
            });

            _notice.AddNotice(account.Id, "Список рекомендованных друзей получен - " + friendListResponseModels.Friends.Count);

            var friendList = friendListResponseModels.Friends.Select(model => new AnalysisFriendData
            {
                AccountId = accountModel.Id,
                FacebookId = model.FacebookId,
                Type = model.Type,
                Status = StatusesFriend.ToAnalys,
                FriendName = model.FriendName
            }).ToList();

            //Check
            _notice.AddNotice(account.Id, "Проверяем не общались ли мы с этими друзьями");

            var certifiedListFriends = _analysisFriendsManager.CheckForAnyInDataBase(accountModel, friendList);

            _notice.AddNotice(account.Id, "Сохраняем рекомендованных друзей");

            new SaveFriendsForAnalysisCommandHandler(new DataBaseContext()).Handle(new SaveFriendsForAnalysisCommand
            {
                AccountId = accountModel.Id,
                Friends = certifiedListFriends
            });

            new AddAccountInformationCommandHandler(new DataBaseContext()).Handle(new AddAccountInformationCommand
            {
                AccountId = account.Id,
                AccountInformationData = new AccountInformationDataDbModel
                {
                    CountIncommingFriendsRequest = friendListResponseModels.CountIncommingFriends
                }
            });

            _notice.AddNotice(account.Id, "Получение рекомендованных друзей завершено.");
            return new NewFriendListViewModel
            {
                AccountId = accountModel.Id,
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

        public void ConfirmFriendship(AccountViewModel account)
        {
            var accountModel = new AccountModel
            {
                AuthorizationDataIsFailed = account.AuthorizationDataIsFailed,
                Cookie = new CookieModel
                {
                    CookieString = account.Cookie
                },
                ProxyDataIsFailed = account.ProxyDataIsFailed,
                Id = account.Id,
                Name = account.Name,
                Proxy = account.Proxy,
                FacebookId = account.FacebookId,
                GroupSettingsId = account.GroupSettingsId,
                Login = account.Login,
                PageUrl = account.PageUrl,
                Password = account.Password,
                ProxyLogin = account.ProxyLogin,
                ProxyPassword = account.ProxyPassword
            };

            _notice.AddNotice(account.Id, "Получаем подходящих друзей для подтверждения дружбы");

            var friends = new GetFriendsToConfirmQueryHandler(new DataBaseContext()).Handle(new GetFriendsToConfirmQuery
            {
                AccountId = account.Id
            });

            if (friends.Count == 0)
            {
                return;
            }

            _notice.AddNotice(account.Id, "Выбираем случайного друга");

            var analysisFriendsData = friends.FirstOrDefault();

            if (analysisFriendsData != null)
            {
                _notice.AddNotice(account.Id,
                    string.Format("Подтверждаем дружбу с {0}({1})", analysisFriendsData.FriendName,
                        analysisFriendsData.FacebookId));

                new ConfirmFriendshipEngine().Execute(new ConfirmFriendshipModel
                {
                    AccountFacebookId = account.FacebookId,
                    FriendFacebookId = analysisFriendsData.FacebookId,
                    Proxy = _accountManager.GetAccountProxy(accountModel),
                    Cookie = account.Cookie,
                    UrlParameters =
                        new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
                        {
                            NameUrlParameter = NamesUrlParameter.ConfirmFriendship
                        }),
                });

                if (true)
                {
                    _notice.AddNotice(account.Id, "Обновляем статистику");

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

                _notice.AddNotice(account.Id, "Обновляем статистику");
            }

            Thread.Sleep(2000);
        }

        public void SendRequestFriendship(AccountViewModel account)
        {
            var accountModel = new AccountModel
            {
                AuthorizationDataIsFailed = account.AuthorizationDataIsFailed,
                Cookie = new CookieModel
                {
                    CookieString = account.Cookie
                },
                ProxyDataIsFailed = account.ProxyDataIsFailed,
                Id = account.Id,
                Name = account.Name,
                Proxy = account.Proxy,
                FacebookId = account.FacebookId,
                GroupSettingsId = account.GroupSettingsId,
                Login = account.Login,
                PageUrl = account.PageUrl,
                Password = account.Password,
                ProxyLogin = account.ProxyLogin,
                ProxyPassword = account.ProxyPassword
            };

            _notice.AddNotice(account.Id, "Получаем подходящих друзей для отправки заявки");

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
                if (analysisFriendsData != null)
                {
                    _notice.AddNotice(account.Id,
                        string.Format("Отправляем заявку {0}({1})", analysisFriendsData.FriendName,
                            analysisFriendsData.FacebookId));

                    new SendRequestFriendshipEngine().Execute(new SendRequestFriendshipModel
                    {
                        AccountFacebookId = account.FacebookId,
                        FriendFacebookId = analysisFriendsData.FacebookId,
                        Proxy = _accountManager.GetAccountProxy(accountModel),
                        Cookie = accountModel.Cookie.CookieString,
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
                        _notice.AddNotice(account.Id, "Обновляем статистику");

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

                    _notice.AddNotice(account.Id, "Отправка заявки в друзья успешно завершена.");
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        public void RemoveFriend(long accountId, long friendId)
        {
            var account = new GetAccountByIdQueryHandler(new DataBaseContext()).Handle(new GetAccountByIdQuery
            {
                UserId = accountId
            });

            _notice.AddNotice(account.Id, string.Format("Получаем друга для удаления по id - {0}", friendId));

            var friend = new GetFriendByIdQueryHandler(new DataBaseContext()).Handle(new GetFriendByIdQuery
            {
                FriendId = friendId
            });

            _notice.AddNotice(account.Id, string.Format("удаляем друга {0}({1})", friend.FriendName, friend.FacebookId));

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

            _notice.AddNotice(account.Id, string.Format("{0}({1}) успешно удалён из друзей", friend.FriendName, friend.FacebookId));

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

            _notice.AddNotice(account.Id, string.Format("Отменяем входящую заявку друга {0}", friendFacebookId));

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

            _notice.AddNotice(account.Id, string.Format("Заявка дружбы друга {0} отменена", friendFacebookId));

            new ChangeAnalysisFriendStatusCommandHandler(new DataBaseContext()).Handle(new ChangeAnalysisFriendStatusCommand
            {
               AccountId = accountId,
               FriendFacebookId = friendFacebookId,
               NewStatus = StatusesFriend.ToDelete
            });
        }
    }
}
