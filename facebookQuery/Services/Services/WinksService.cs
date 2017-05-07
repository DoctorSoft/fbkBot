using System;
using System.Collections.Generic;
using System.Linq;
using DataBase.Constants;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.AccountStatistics;
using DataBase.QueriesAndCommands.Commands.Friends.MarkWinkedFriendCommand;
using DataBase.QueriesAndCommands.Commands.Friends.MarkWinksFriendsFriendsCommand;
using DataBase.QueriesAndCommands.Queries.Account.Models;
using DataBase.QueriesAndCommands.Queries.Friends.GetFriendsToWink;
using DataBase.QueriesAndCommands.Queries.Friends.GetFriendsToWinkFriendsFriends;
using DataBase.QueriesAndCommands.Queries.UrlParameters;
using DataBase.QueriesAndCommands.Queries.UserAgent.GetUserAgentById;
using Engines.Engines.GetFriendsEngine.GetRandomFriendFriends;
using Engines.Engines.GetNewWinks;
using Engines.Engines.WinkEngine;
using Services.Interfaces.Notices;
using Services.Interfaces.ServiceTools;
using Services.ServiceTools;
using Services.ViewModels.FriendsModels;
using Services.ViewModels.HomeModels;

namespace Services.Services
{
    public class WinksService
    {
        private readonly INoticesProxy _notice;
        private readonly IAccountManager _accountManager;
        private readonly NoticeService _noticesService;
        private readonly IFriendManager _friendManager;
        private readonly IAccountSettingsManager _accountSettingsManager;
        private readonly IStatisticsManager _accountStatisticsManager;
        private readonly ISeleniumManager _seleniumManager;
        private readonly IAnalysisFriendsManager _analysisFriendsManager;

        private const int CountFriendsToGet = 10;

        public WinksService(INoticesProxy noticeProxy)
        {
            _noticesService = new NoticeService();
            _notice = noticeProxy;
            _accountManager = new AccountManager();
            _friendManager = new FriendManager();
            _accountSettingsManager = new AccountSettingsManager();
            _accountStatisticsManager = new StatisticsManager();
            _seleniumManager = new SeleniumManager();
            _analysisFriendsManager = new AnalysisFriendsManager();
        }

        public FriendViewModel GetFriendToWink(AccountViewModel account)
        {
            _notice.AddNotice(account.Id, "Получаем друга для подмигиваний");
            var groupId = account.GroupSettingsId;

            if (groupId == null)
            {
                return null;
            }

            var settings = _accountSettingsManager.GetSettings((long)groupId);
            
            var model = new GetFriendToWinkQueryHandler(new DataBaseContext()).Handle(new GetFriendToWinkQuery
            {
                AccountId = account.Id,
                GroupSettingsId = (long)groupId
            });

            if (model == null)
            {
                _notice.AddNotice(account.Id, string.Format("Нет ниодного подходящего друга."));
                return null;
            }

            _notice.AddNotice(account.Id, string.Format("Получили {0}({1})", model.FriendName, model.FacebookId));

            var testedFriendsId = new List<long> {model.FacebookId};

            var considerGeoForWink = settings.ConsiderGeoForWinkFriends;
            if (!considerGeoForWink)
            {
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

            var driver = _seleniumManager.RegisterNewDriver(account); // открываем драйвер
            
            _notice.AddNotice(account.Id, string.Format("Проверяем гео данные для {0}({1})", model.FriendName, model.FacebookId));

            var isSuccces = new SpyService(null).AnalizeFriend(account, model.FacebookId, settings, driver);
           
            while (!isSuccces && settings.ConsiderGeoForWinkFriends)
            {
                _notice.AddNotice(account.Id, string.Format("Не прошел провергу по Гео {0}({1}), берем другого", model.FriendName, model.FacebookId));
                model = new GetFriendToWinkQueryHandler(new DataBaseContext()).Handle(new GetFriendToWinkQuery
                {
                    AccountId = account.Id,
                    GroupSettingsId = (long)groupId,
                    TestedFriendsId = testedFriendsId
                });

                testedFriendsId.Add(model.FacebookId);

                _notice.AddNotice(account.Id, string.Format("Проверяем гео данные для {0}({1})", model.FriendName, model.FacebookId));

                isSuccces = new SpyService(null).AnalizeFriend(account, model.FacebookId, settings, driver);

                settings = _accountSettingsManager.GetSettings((long)groupId); //Обновляем настройки
            }

            driver.Quit(); //Закрываем драйвер

            _notice.AddNotice(account.Id, string.Format("Получили {0}({1})", model.FriendName, model.FacebookId));
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

        public void WinkFriend(AccountViewModel account)
        {
            const string functionName = "Подмигнуть";

            try
            {
                if (account.GroupSettingsId == null)
                {
                    return;
                }

                var friend = GetFriendToWink(account);

                if (friend == null)
                {
                    _notice.AddNotice(account.Id, _noticesService.ConvertNoticeText(functionName, string.Format("Нет друзей для подмигивания")));
                    return;
                }

                _notice.AddNotice(account.Id,
                    _noticesService.ConvertNoticeText(functionName,string.Format("Подмигиваем другу {0}({1})", friend.Name, friend.FacebookId)));
                new WinkEngine().Execute(new WinkModel
                {
                    AccountFacebookId = account.FacebookId,
                    Proxy = _accountManager.GetAccountProxy(account),
                    Cookie = account.Cookie,
                    FriendFacebookId = friend.FacebookId,
                    UrlParameters =
                        new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
                        {
                            NameUrlParameter = NamesUrlParameter.Wink
                        })
                });

                _notice.AddNotice(account.Id,
                    _noticesService.ConvertNoticeText(functionName,string.Format("Делаем отметку о подмигивании другу {0}({1})", friend.Name, friend.FacebookId)));
                new MarkWinkedFriendCommandHandler(new DataBaseContext()).Handle(new MarkWinkedFriendCommand
                {
                    FriendId = friend.Id,
                    AccountId = account.Id
                });

            }
            catch (Exception ex)
            {
                _notice.AddNotice(account.Id,
                    _noticesService.ConvertNoticeText(functionName, string.Format("Произошла оибка - {0}", ex.Message)));
            }
        }

        public void WinkFriendsFriends(AccountViewModel account)
        {
            const string functionName = "Подмигнуть друзьям друзей";

            try
            {
                _notice.AddNotice(account.Id,
                    _noticesService.ConvertNoticeText(functionName,
                        string.Format("Начинаем подмигивать друзьям друзей.")));
                if (account.GroupSettingsId == null)
                {
                    return;
                }

                var friends =
                    new GetFriendsToWinkFriendsFriendsQueryHandler(new DataBaseContext()).Handle(
                        new GetFriendsToWinkFriendsFriendsQuery
                        {
                            AccountId = account.Id,
                            GroupSettingsId = (long) account.GroupSettingsId,
                            CountFriends = 5
                        });

                if (friends == null)
                {
                    _notice.AddNotice(account.Id,
                        _noticesService.ConvertNoticeText(functionName,
                            string.Format("Друзья для подмигивания не найдены.")));
                    return;
                }

                _notice.AddNotice(account.Id,
                    _noticesService.ConvertNoticeText(functionName, string.Format("Получаем друзей друзей...")));

                var friendsFriendsToWink = new GetRandomFriendFriendsEngine().Execute(
                    new GetRandomFriendFriendsModel
                    {
                        Proxy = _accountManager.GetAccountProxy(account),
                        Cookie = account.Cookie,
                        AccountFacebookId = account.FacebookId,
                        Driver = _seleniumManager.RegisterNewDriver(account),
                        FriendsIdList = friends.Select(model => new GetRandomFriendModel
                        {
                            FriendId = model.FriendId,
                            FriendFacebookId = model.FriendFacebookId
                        }).ToList()
                    });

                if (friendsFriendsToWink == null)
                {
                    _notice.AddNotice(account.Id,
                        _noticesService.ConvertNoticeText(functionName,
                            string.Format("Друзья друзей для подмигивания не найдены.")));
                    return;
                }

                _notice.AddNotice(account.Id,
                    _noticesService.ConvertNoticeText(functionName,
                        string.Format("Друзья друзей получены, начинаем подмигивать.")));

                var settings = _accountSettingsManager.GetSettings((long) account.GroupSettingsId);
                if (settings.ConsiderGeoForWinkFriendsFriends)
                {
                    var driver = _seleniumManager.RegisterNewDriver(account); // открываем драйвер
                    try
                    {
                        foreach (var friendFacebook in friendsFriendsToWink)
                        {
                            _notice.AddNotice(account.Id,
                                _noticesService.ConvertNoticeText(functionName,
                                    string.Format("Проверяем Гео для ({0})", friendFacebook.FriendFriendFacebookId)));

                            var isSuccces = new SpyService(null).AnalizeFriend(account,
                                friendFacebook.FriendFriendFacebookId,
                                settings, driver);

                            if (isSuccces)
                            {
                                _notice.AddNotice(account.Id,
                                    _noticesService.ConvertNoticeText(functionName,
                                        string.Format("Подмигиваем другу друга({0})",
                                            friendFacebook.FriendFriendFacebookId)));

                                new WinkEngine().Execute(new WinkModel
                                {
                                    AccountFacebookId = account.FacebookId,
                                    Proxy = _accountManager.GetAccountProxy(account),
                                    Cookie = account.Cookie,
                                    FriendFacebookId = Convert.ToInt64(friendFacebook.FriendFriendFacebookId),
                                    UrlParameters =
                                        new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
                                        {
                                            NameUrlParameter = NamesUrlParameter.Wink
                                        })
                                });

                                new MarkWinksFriendsFriendsCommandHandler(new DataBaseContext()).Handle(new MarkWinksFriendsFriendsCommand
                                {
                                    FriendId = friendFacebook.FriendId,
                                    AccountId = account.Id
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _notice.AddNotice(account.Id,
                            _noticesService.ConvertNoticeText(functionName,
                                string.Format("Произошла оибка - {0}", ex.Message)));
                    }
                    driver.Quit();
                }
                else
                {
                    foreach (var friendFacebook in friendsFriendsToWink)
                    {
                        _notice.AddNotice(account.Id,
                            _noticesService.ConvertNoticeText(functionName,
                                string.Format("Подмигиваем другу друга({0})", friendFacebook.FriendFriendFacebookId)));
                        new WinkEngine().Execute(new WinkModel
                        {
                            AccountFacebookId = account.FacebookId,
                            Proxy = _accountManager.GetAccountProxy(account),
                            Cookie = account.Cookie,
                            FriendFacebookId = Convert.ToInt64(friendFacebook.FriendFriendFacebookId),
                            UrlParameters =
                                new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
                                {
                                    NameUrlParameter = NamesUrlParameter.Wink
                                })
                        });

                        new MarkWinksFriendsFriendsCommandHandler(new DataBaseContext()).Handle(new MarkWinksFriendsFriendsCommand
                        {
                            FriendId = friendFacebook.FriendId,
                            AccountId = account.Id
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _notice.AddNotice(account.Id,
                    _noticesService.ConvertNoticeText(functionName, string.Format("Произошла оибка - {0}", ex.Message)));
            }
        }

        public void WinkToBack(AccountViewModel account)
        {
            const string functionName = "Подмигнуть в ответ";

            try
            {
                _notice.AddNotice(account.Id, _noticesService.ConvertNoticeText(functionName, string.Format("Начинаем подмигивать в ответ")));

                var userAgent = new GetUserAgentQueryHandler(new DataBaseContext()).Handle(new GetUserAgentQuery
                {
                    UserAgentId = account.UserAgentId
                });

                var countPokes = new GetNewWinksEngine().Execute(new GetNewWinksModel
                {
                    Proxy = _accountManager.GetAccountProxy(account),
                    Cookie = account.Cookie,
                    UserAgent = userAgent.UserAgentString
                });

                if (countPokes != 0)
                {
                    _notice.AddNotice(account.Id, _noticesService.ConvertNoticeText(functionName, string.Format("Подмигнули {0} пользователям в ответ", countPokes)));

                    new AddOrUpdateAccountStatisticsCommandHandler(new DataBaseContext()).Handle(
                        new AddOrUpdateAccountStatisticsCommand
                        {
                            AccountId = account.Id,
                            CountOfWinksBack = countPokes
                        });

                    return;
                }

                _notice.AddNotice(account.Id, _noticesService.ConvertNoticeText(functionName, string.Format("Нет подмигиваний.")));
            }
            catch (Exception ex)
            {
                _notice.AddNotice(account.Id, _noticesService.ConvertNoticeText(functionName,string.Format("Произошла ошибка - {0}", ex.Message)));
            }
        }
    }
}
