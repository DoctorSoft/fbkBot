using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonInterfaces.Interfaces.Services;
using CommonModels;
using DataBase.Constants;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.CommunityStatistics;
using DataBase.QueriesAndCommands.Commands.Friends.MarkAddToGroupFriendCommand;
using DataBase.QueriesAndCommands.Commands.Groups;
using DataBase.QueriesAndCommands.Commands.NewSettings;
using DataBase.QueriesAndCommands.Commands.Settings;
using DataBase.QueriesAndCommands.Models;
using DataBase.QueriesAndCommands.Models.ConditionModels;
using DataBase.QueriesAndCommands.Models.JsonModels;
using DataBase.QueriesAndCommands.Queries.Account;
using DataBase.QueriesAndCommands.Queries.Account.Models;
using DataBase.QueriesAndCommands.Queries.CommunityStatistics;
using DataBase.QueriesAndCommands.Queries.Friends.GetFriendsForAddedToGroup;
using DataBase.QueriesAndCommands.Queries.Groups;
using DataBase.QueriesAndCommands.Queries.NewSettings;
using DataBase.QueriesAndCommands.Queries.Settings;
using DataBase.QueriesAndCommands.Queries.UrlParameters;
using Engines.Engines.AddToGroupEngine;
using Engines.Engines.AddToPageEngine;
using Engines.Engines.JoinTheGroupsAndPagesEngine.JoinTheGroupsBySeleniumEngine;
using Engines.Engines.JoinTheGroupsAndPagesEngine.JoinThePagesBySeleniumEngine;
using Services.Interfaces.Notices;
using Services.Models.BackgroundJobs;
using Services.ServiceTools;
using Services.ViewModels.GroupModels;
using Services.ViewModels.HomeModels;
using Services.ViewModels.NewSettingsModels;

namespace Services.Services
{
    public class GroupService
    {
        private readonly INoticesProxy _notice;
        private readonly AccountSettingsManager _accountSettingsManager;
        private readonly SeleniumManager _seleniumManager;
        private readonly SettingsManager _settingsManager;
        private readonly AccountManager _accountManager;

        public GroupService(INoticesProxy notice)
        {
            _notice = notice;
            _accountSettingsManager = new AccountSettingsManager();
            _seleniumManager = new SeleniumManager();
            _settingsManager = new SettingsManager();
            _accountManager = new AccountManager();
        }
        public GroupList GetGroups()
        {
            var groups = new GetGroupsQueryHandler(new DataBaseContext()).Handle(new GetGroupsQuery());

            return new GroupList
            {
                Groups = groups.Select(data => new Group
                {
                    Id = data.Id,
                    Name = data.Name
                }).ToList()
            };
        }

        public void AddNewGroup(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            new AddNewGroupCommandHandler(new DataBaseContext()).Handle(new AddNewGroupCommand
            {
                Name = name
            });
        }

        public void RemoveGroup(long groupId)
        {
            new RemoveGroupCommandHandler(new DataBaseContext()).Handle(new RemoveGroupCommand
            {
                Id = groupId
            });
        }

        public void UpdateGroup(long groupId, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            new UpdateGroupCommandHandler(new DataBaseContext()).Handle(new UpdateGroupCommand
            {
                Name = name,
                Id = groupId
            });
        }

        public void InviteToGroup(AccountViewModel account)
        {
            _notice.AddNotice(account.Id, "Отправляем приглашения в группы...");

            if (account.GroupSettingsId == null)
            {
                return;
            }

            var settings = _accountSettingsManager.GetSettings((long) account.GroupSettingsId);
            var statistics =
                new GetCommunityStatisticsQueryHandler(new DataBaseContext()).Handle(new GetCommunityStatisticsQuery
                {
                    GroupId = (long) account.GroupSettingsId
                });

            var limitMinJoinGroupLastDay = settings.MinFriendsJoinGroupInDay;
            var limitMaxJoinGroupLastDay = settings.MaxFriendsJoinGroupInDay;
            var limitMinJoinGroupLastHour = settings.MinFriendsJoinGroupInHour;
            var limitMaxJoinGroupLastHour = settings.MaxFriendsJoinGroupInHour;

            var countJoinGroupLastDay = statistics.Sum(data => data.CountOfGroupInvitations);
            var countJoinGroupLastHour =
                statistics.Where(
                    model =>
                        (DateTime.Now - model.UpdateDateTime).Hours*60 + (DateTime.Now - model.UpdateDateTime).Minutes <
                        60).Sum(data => data.CountOfGroupInvitations);

            if (countJoinGroupLastDay >= limitMaxJoinGroupLastDay)
            {
                _notice.AddNotice(account.Id, "Достигнут лимит максимального количества добавлений в группу в день!");
                return;
            }

            if (countJoinGroupLastHour >= limitMaxJoinGroupLastHour)
            {
                _notice.AddNotice(account.Id, "Достигнут лимит максимального количества добавлений в группу в час!");
                return;
            }

            var allowedToInviteHour = limitMaxJoinGroupLastHour - countJoinGroupLastHour;
            var allowedToInviteDay = limitMaxJoinGroupLastDay - countJoinGroupLastDay;


            var friends =
                new GetFriendsForAddedToGroupQueryHandler(new DataBaseContext()).Handle(new GetFriendsForAddedToGroupQuery
                {
                    AccountId = account.Id,
                    Count =
                        (int)
                            (allowedToInviteDay < allowedToInviteHour ? allowedToInviteDay : limitMaxJoinGroupLastHour)
                });

            if (friends.Count < limitMinJoinGroupLastHour)
            {
                _notice.AddNotice(account.Id, string.Format("Недостаточно людей для добавления! Получено {0} людей для добавления! Мин. кол-во - {1}", friends.Count, limitMinJoinGroupLastHour));
                return;
            }

            var groups = ConvertStringToList(settings.FacebookGroups);

            _notice.AddNotice(account.Id, string.Format("Начинаем добавлять {0} друзей в группы", friends.Count));

            if (friends.Count > 50)
            {
                _notice.AddNotice(account.Id, string.Format("Друзей оказалось больше, берем 50 из них."));
                friends = new List<FriendData>(friends.Take(50));
            }

            foreach (var @group in groups)
            {
                _notice.AddNotice(account.Id,
                    string.Format("Добавляем {0} друзей в группу - {1}", friends.Count, @group));
                new AddToGroupEngine().Execute(new AddToGroupModel
                {
                    Cookie = account.Cookie,
                    FacebookId = account.FacebookId,
                    Proxy = _accountManager.GetAccountProxy(new AccountModel
                    {
                        Proxy = account.Proxy,
                        ProxyLogin = account.ProxyLogin,
                        ProxyPassword = account.ProxyPassword
                    }),
                    FacebookGroupUrl = @group,
                    FriendsList = friends.Select(data => new FriendModel
                    {
                        FacebookId = data.FacebookId,
                        FriendName = data.FriendName
                    }).ToList(),
                    UrlParameters =
                        new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
                        {
                            NameUrlParameter = NamesUrlParameter.AddFriendsToGroup
                        })
                });
            }

            foreach (var friend in friends)
            {
                _notice.AddNotice(account.Id, string.Format("Отмечаем друзей как добавленные в группы"));
                new MarkAddToGroupFriendCommandHandler(new DataBaseContext()).Handle(new MarkAddToGroupFriendCommand
                {
                    AccountId = account.Id,
                    FriendId = friend.Id
                });
            }
            
            _notice.AddNotice(account.Id, string.Format("Добавлено {0} друзей в группы", friends.Count));
            new AddCommunityStatisticsCommandHandler(new DataBaseContext()).Handle(new AddCommunityStatisticsCommand
            {
                AccountId = account.Id,
                GroupId = (long)account.GroupSettingsId,
                CountOfGroupInvitations = friends.Count
            });

        }

        public void InviteToPage(AccountViewModel account)
        {
            _notice.AddNotice(account.Id, "Отправляем приглашения в сообщества...");

            if (account.GroupSettingsId == null)
            {
                return;
            }

            var settings = _accountSettingsManager.GetSettings((long)account.GroupSettingsId);
            var statistics =
                new GetCommunityStatisticsQueryHandler(new DataBaseContext()).Handle(new GetCommunityStatisticsQuery
                {
                    GroupId = (long)account.GroupSettingsId
                });

            var limitMinJoinPageLastDay = settings.MinFriendsJoinPageInDay;
            var limitMaxJoinPageLastDay = settings.MaxFriendsJoinPageInDay;
            var limitMinJoinPageLastHour = settings.MinFriendsJoinPageInHour;
            var limitMaxJoinPageLastHour = settings.MaxFriendsJoinPageInHour;

            var countJoinPageLastDay = statistics.Sum(data => data.CountOfPageInvitations);
            var countJoinPageLastHour =
                statistics.Where(
                    model =>
                        (DateTime.Now - model.UpdateDateTime).Hours * 60 + (DateTime.Now - model.UpdateDateTime).Minutes <
                        60).Sum(data => data.CountOfPageInvitations);

            if (countJoinPageLastDay >= limitMaxJoinPageLastDay)
            {
                _notice.AddNotice(account.Id, "Превышен лимит максимального количества добавлений в сообщества в день!");
                return;
            }

            if (countJoinPageLastHour >= limitMaxJoinPageLastHour)
            {
                _notice.AddNotice(account.Id, "Превышен лимит максимального количества добавлений в сообщества в час!");
                return;
            }

            var allowedToInviteHour = limitMaxJoinPageLastHour - countJoinPageLastHour;
            var allowedToInviteDay = limitMaxJoinPageLastDay - countJoinPageLastDay;


            var friends =
                new GetFriendsForAddedToGroupQueryHandler(new DataBaseContext()).Handle(new GetFriendsForAddedToGroupQuery
                {
                    AccountId = account.Id,
                    Count = (int)(allowedToInviteDay < allowedToInviteHour ? allowedToInviteDay : limitMaxJoinPageLastHour)
                });

            if (friends.Count < limitMinJoinPageLastHour)
            {
                _notice.AddNotice(account.Id, string.Format("Недостаточно людей для добавления в сообщество! Получено {0} людей для добавления! Мин. кол-во - {1}", friends.Count, limitMinJoinPageLastHour));
                return;
            }

            var pages = ConvertStringToList(settings.FacebookPages);


            _notice.AddNotice(account.Id, string.Format("Начинаем добавлять {0} друзей в сообщества", friends.Count));
            foreach (var friendData in friends)
            {
                foreach (var page in pages)
                {
                    _notice.AddNotice(account.Id, string.Format("Добавляем {0} в в сообщество {1}", friendData.FriendName, page));
                    new AddToPageEngine().Execute(new AddToPageModel
                    {
                        Cookie = account.Cookie,
                        FacebookId = account.FacebookId,
                        Proxy = _accountManager.GetAccountProxy(new AccountModel
                        {
                            Proxy = account.Proxy,
                            ProxyLogin = account.ProxyLogin,
                            ProxyPassword = account.ProxyPassword
                        }),
                        FacebookPageUrl = page,
                        Friend =  new FriendModel
                        {
                            FacebookId = friendData.FacebookId,
                            FriendName = friendData.FriendName
                        },
                        UrlParameters =
                            new GetUrlParametersQueryHandler(new DataBaseContext()).Handle(new GetUrlParametersQuery
                            {
                                NameUrlParameter = NamesUrlParameter.AddFriendsToPage
                            })
                    });
                }

                _notice.AddNotice(account.Id, string.Format("Отмечаем друга {0} как добавленого в сообщества", friendData.FriendName));
                new MarkAddToGroupFriendCommandHandler(new DataBaseContext()).Handle(new MarkAddToGroupFriendCommand
                {
                    AccountId = account.Id,
                    FriendId = friendData.Id
                });
            }

            _notice.AddNotice(account.Id, string.Format("Добавлено {0} друзей в сообщеста", friends.Count));
            new AddCommunityStatisticsCommandHandler(new DataBaseContext()).Handle(new AddCommunityStatisticsCommand
            {
                AccountId = account.Id,
                GroupId = (long)account.GroupSettingsId,
                CountOfPageInvitations = friends.Count
            });
        }

        public void JoinTheNewGroupsAndPages(AccountViewModel account)
        {
            _notice.AddNotice(account.Id, "Отправляем запросы на присоединение аккаунта к группам и страницам...");

            if (account.GroupSettingsId == null)
            {
                return;
            }

            var communities =
                new GetNewSettingsByAccountAndGroupIdQueryHandler(new DataBaseContext()).Handle(
                    new GetNewSettingsByAccountAndGroupIdQuery
                    {
                        AccountId = account.Id,
                        GroupId = (long)account.GroupSettingsId
                    });

            new DeleteNewSettingsCommandHandler(new DataBaseContext()).Handle(new DeleteNewSettingsCommand
            {
                AccountId = account.Id,
                GroupId = (long)account.GroupSettingsId
            });

            foreach (var newSettingsData in communities)
            {
                _notice.AddNotice(account.Id, string.Format("В очереди {0} новых групп", newSettingsData.CommunityOptions.Groups.Count));

                _notice.AddNotice(account.Id, string.Format("Начинаем отправлять запросы на присоединения к группам"));

                new JoinTheGroupsBySeleniumEngine().Execute(new JoinTheGroupsBySeleniumModel
                {
                    Driver = _seleniumManager.RegisterNewDriver(account),
                    Cookie = account.Cookie,
                    Groups = newSettingsData.CommunityOptions.Groups
                });

                _notice.AddNotice(account.Id, string.Format("В очереди {0} новых страниц", newSettingsData.CommunityOptions.Pages.Count));

                _notice.AddNotice(account.Id, string.Format("Начинаем отправлять запросы на присоединения к страницам"));
               
                new JoinThePagesBySeleniumEngine().Execute(new JoinThePagesBySeleniumModel
                {
                    Driver = _seleniumManager.RegisterNewDriver(account),
                    Cookie = account.Cookie,
                    Pages = newSettingsData.CommunityOptions.Pages
                });
            }

            _notice.AddNotice(account.Id, string.Format("Присоединения к группам и страницам успешно завершено"));
        }

        public List<NewSettingsViewModel> GetNewSettings(long accountId, long groupId)
        {
            var settingsDbModels =
                new GetNewSettingsByAccountAndGroupIdQueryHandler(new DataBaseContext()).Handle(
                    new GetNewSettingsByAccountAndGroupIdQuery
                    {
                        AccountId = accountId,
                        GroupId = groupId
                    });

            var settingsModels = settingsDbModels.Select(data => new NewSettingsViewModel
            {
                AccountId = data.AccountId,
                Id = data.Id,
                SettingsGroupId = data.SettingsGroupId,
                CommunityOptions = new CommunityOptionsViewModel
                {
                    Groups = data.CommunityOptions.Groups,
                    Pages = data.CommunityOptions.Pages
                }
            }).ToList();

            return settingsModels;
        }
        public GroupSettingsViewModel GetSettings(long groupId)
        {
            var settings =
                new GetSettingsByGroupSettingsIdQueryHandler(new DataBaseContext()).Handle(new GetSettingsByGroupSettingsIdQuery
                {
                    GroupSettingsId = groupId
                });

            if (settings == null)
            {
                return new GroupSettingsViewModel()
                {
                    GroupId = groupId
                };
            }

            return new GroupSettingsViewModel
            {
                GroupId = settings.GroupId,
                IsJoinToAllGroups = settings.CommunityOptions.IsJoinToAllGroups,
                RetryTimeInviteTheGroupsHour = settings.CommunityOptions.RetryTimeInviteTheGroups == null ? 0 : settings.CommunityOptions.RetryTimeInviteTheGroups.Hours,
                RetryTimeInviteTheGroupsMin = settings.CommunityOptions.RetryTimeInviteTheGroups == null ? 0 : settings.CommunityOptions.RetryTimeInviteTheGroups.Minutes,
                RetryTimeInviteTheGroupsSec = settings.CommunityOptions.RetryTimeInviteTheGroups == null ? 0 : settings.CommunityOptions.RetryTimeInviteTheGroups.Seconds,
                RetryTimeInviteThePagesHour = settings.CommunityOptions.RetryTimeInviteThePages == null ? 0 : settings.CommunityOptions.RetryTimeInviteThePages.Hours,
                RetryTimeInviteThePagesMin = settings.CommunityOptions.RetryTimeInviteThePages == null ? 0 : settings.CommunityOptions.RetryTimeInviteThePages.Minutes,
                RetryTimeInviteThePagesSec = settings.CommunityOptions.RetryTimeInviteThePages == null ? 0 : settings.CommunityOptions.RetryTimeInviteThePages.Seconds,
                FacebookGroups = ConvertListToString(settings.CommunityOptions.Groups),
                FacebookPages = ConvertListToString(settings.CommunityOptions.Pages),
                MaxFriendsJoinGroupInDay = settings.CommunityOptions.MaxFriendsJoinGroupInDay,
                MinFriendsJoinGroupInDay = settings.CommunityOptions.MinFriendsJoinGroupInDay,
                MaxFriendsJoinGroupInHour = settings.CommunityOptions.MaxFriendsJoinGroupInHour,
                MinFriendsJoinGroupInHour = settings.CommunityOptions.MinFriendsJoinGroupInHour,
                MaxFriendsJoinPageInDay = settings.CommunityOptions.MaxFriendsJoinPageInDay,
                MinFriendsJoinPageInDay = settings.CommunityOptions.MinFriendsJoinPageInDay,
                MaxFriendsJoinPageInHour = settings.CommunityOptions.MaxFriendsJoinPageInHour,
                MinFriendsJoinPageInHour = settings.CommunityOptions.MinFriendsJoinPageInHour,
                Gender = settings.GeoOptions.Gender,
                Cities = settings.GeoOptions.Cities,
                Countries = settings.GeoOptions.Countries,
                RetryTimeSendUnreadHour = settings.MessageOptions.RetryTimeSendUnread.Hours,
                RetryTimeSendUnreadMin = settings.MessageOptions.RetryTimeSendUnread.Minutes,
                RetryTimeSendUnreadSec = settings.MessageOptions.RetryTimeSendUnread.Seconds,
                RetryTimeConfirmFriendshipsHour = settings.FriendsOptions.RetryTimeConfirmFriendships.Hours,
                RetryTimeConfirmFriendshipsMin = settings.FriendsOptions.RetryTimeConfirmFriendships.Minutes,
                RetryTimeConfirmFriendshipsSec = settings.FriendsOptions.RetryTimeConfirmFriendships.Seconds,
                RetryTimeGetNewAndRecommendedFriendsHour = settings.FriendsOptions.RetryTimeGetNewAndRecommendedFriends.Hours,
                RetryTimeGetNewAndRecommendedFriendsMin = settings.FriendsOptions.RetryTimeGetNewAndRecommendedFriends.Minutes,
                RetryTimeGetNewAndRecommendedFriendsSec = settings.FriendsOptions.RetryTimeGetNewAndRecommendedFriends.Seconds,
                RetryTimeRefreshFriendsHour = settings.FriendsOptions.RetryTimeRefreshFriends.Hours,
                RetryTimeRefreshFriendsMin = settings.FriendsOptions.RetryTimeRefreshFriends.Minutes,
                RetryTimeRefreshFriendsSec = settings.FriendsOptions.RetryTimeRefreshFriends.Seconds,
                RetryTimeSendNewFriendHour = settings.MessageOptions.RetryTimeSendNewFriend.Hours,
                RetryTimeSendNewFriendMin = settings.MessageOptions.RetryTimeSendNewFriend.Minutes,
                RetryTimeSendNewFriendSec = settings.MessageOptions.RetryTimeSendNewFriend.Seconds,
                RetryTimeSendRequestFriendshipsHour = settings.FriendsOptions.RetryTimeSendRequestFriendships.Hours,
                RetryTimeSendRequestFriendshipsMin = settings.FriendsOptions.RetryTimeSendRequestFriendships.Minutes,
                RetryTimeSendRequestFriendshipsSec = settings.FriendsOptions.RetryTimeSendRequestFriendships.Seconds,
                RetryTimeSendUnansweredHour = settings.MessageOptions.RetryTimeSendUnanswered.Hours,
                RetryTimeSendUnansweredMin = settings.MessageOptions.RetryTimeSendUnanswered.Minutes,
                RetryTimeSendUnansweredSec = settings.MessageOptions.RetryTimeSendUnanswered.Seconds,
                UnansweredDelay = settings.MessageOptions.UnansweredDelay,
                
                //limits options
                CountMaxFriends = settings.LimitsOptions.CountMaxFriends,
                CountMinFriends = settings.LimitsOptions.CountMinFriends,

                //delete friends settings 
                DialogIsOverTimer = settings.DeleteFriendsOptions.DialogIsOver == null ? 0 : settings.DeleteFriendsOptions.DialogIsOver.Timer,
                EnableDialogIsOver = settings.DeleteFriendsOptions.DialogIsOver != null && settings.DeleteFriendsOptions.DialogIsOver.IsEnabled,
                IsAddedToGroupsAndPagesTimer = settings.DeleteFriendsOptions.IsAddedToGroupsAndPages == null ? 0 : settings.DeleteFriendsOptions.IsAddedToGroupsAndPages.Timer,
                EnableIsAddedToGroupsAndPages = settings.DeleteFriendsOptions.IsAddedToGroupsAndPages != null && settings.DeleteFriendsOptions.IsAddedToGroupsAndPages.IsEnabled,
                IsWinkTimer = settings.DeleteFriendsOptions.IsWink == null ? 0 : settings.DeleteFriendsOptions.IsWink.Timer,
                EnableIsWink = settings.DeleteFriendsOptions.IsWink != null && settings.DeleteFriendsOptions.IsWink.IsEnabled,
                IsWinkFriendsOfFriendsTimer = settings.DeleteFriendsOptions.IsWinkFriendsOfFriends == null ? 0 : settings.DeleteFriendsOptions.IsWinkFriendsOfFriends.Timer,
                EnableIsWinkFriendsOfFriends = settings.DeleteFriendsOptions.IsWinkFriendsOfFriends != null && settings.DeleteFriendsOptions.IsWinkFriendsOfFriends.IsEnabled
            };
        }

        public void UpdateSettings(GroupSettingsViewModel newSettings, IBackgroundJobService backgroundJobService)
        {
            var oldSettings = _accountSettingsManager.GetSettings(newSettings.GroupId);

            var communityOptions = new CommunityOptionsDbModel
            {
                Groups = ConvertStringToList(newSettings.FacebookGroups),
                Pages = ConvertStringToList(newSettings.FacebookPages),
                IsJoinToAllGroups = newSettings.IsJoinToAllGroups,
                RetryTimeInviteTheGroups = new TimeModel
                {
                    Hours = newSettings.RetryTimeInviteTheGroupsHour,
                    Minutes = newSettings.RetryTimeInviteTheGroupsMin,
                    Seconds = newSettings.RetryTimeInviteTheGroupsSec
                },
                RetryTimeInviteThePages = new TimeModel
                {
                    Hours = newSettings.RetryTimeInviteThePagesHour,
                    Minutes = newSettings.RetryTimeInviteThePagesMin,
                    Seconds = newSettings.RetryTimeInviteThePagesSec
                },
                MaxFriendsJoinGroupInDay = newSettings.MaxFriendsJoinGroupInDay,
                MinFriendsJoinGroupInDay = newSettings.MinFriendsJoinGroupInDay,
                MaxFriendsJoinGroupInHour = newSettings.MaxFriendsJoinGroupInHour,
                MinFriendsJoinGroupInHour = newSettings.MinFriendsJoinGroupInHour,
                MaxFriendsJoinPageInDay = newSettings.MaxFriendsJoinPageInDay,
                MinFriendsJoinPageInDay = newSettings.MinFriendsJoinPageInDay,
                MaxFriendsJoinPageInHour = newSettings.MaxFriendsJoinPageInHour,
                MinFriendsJoinPageInHour = newSettings.MinFriendsJoinPageInHour,
            };

            var geoOptions = new GeoOptionsDbModel
            {
                Cities = string.IsNullOrEmpty(newSettings.Cities) ? string.Empty : newSettings.Cities,
                Countries = string.IsNullOrEmpty(newSettings.Countries) ? string.Empty : newSettings.Countries,
                Gender = newSettings.Gender
            };

            var friendsOptions = new FriendOptionsDbModel
            {
                RetryTimeConfirmFriendships = new TimeModel
                {
                    Hours = newSettings.RetryTimeConfirmFriendshipsHour,
                    Minutes = newSettings.RetryTimeConfirmFriendshipsMin,
                    Seconds = newSettings.RetryTimeConfirmFriendshipsSec
                },
                RetryTimeGetNewAndRecommendedFriends = new TimeModel
                {
                    Hours = newSettings.RetryTimeGetNewAndRecommendedFriendsHour,
                    Minutes = newSettings.RetryTimeGetNewAndRecommendedFriendsMin,
                    Seconds = newSettings.RetryTimeGetNewAndRecommendedFriendsSec
                },
                RetryTimeRefreshFriends = new TimeModel
                {
                    Hours = newSettings.RetryTimeRefreshFriendsHour,
                    Minutes = newSettings.RetryTimeRefreshFriendsMin,
                    Seconds = newSettings.RetryTimeRefreshFriendsSec
                },
                RetryTimeSendRequestFriendships = new TimeModel
                {
                    Hours = newSettings.RetryTimeSendRequestFriendshipsHour,
                    Minutes = newSettings.RetryTimeSendRequestFriendshipsMin,
                    Seconds = newSettings.RetryTimeSendRequestFriendshipsSec
                },
            };

            var messageOptions = new MessageOptionsDbModel
            {
                RetryTimeSendNewFriend = new TimeModel
                {
                    Hours = newSettings.RetryTimeSendNewFriendHour,
                    Minutes = newSettings.RetryTimeSendNewFriendMin,
                    Seconds = newSettings.RetryTimeSendNewFriendSec
                },
                RetryTimeSendUnanswered = new TimeModel
                {
                    Hours = newSettings.RetryTimeSendUnansweredHour,
                    Minutes = newSettings.RetryTimeSendUnansweredMin,
                    Seconds = newSettings.RetryTimeSendUnansweredSec
                },
                RetryTimeSendUnread = new TimeModel
                {
                    Hours = newSettings.RetryTimeSendUnreadHour,
                    Minutes = newSettings.RetryTimeSendUnreadMin,
                    Seconds = newSettings.RetryTimeSendUnreadSec
                },
                UnansweredDelay = newSettings.UnansweredDelay
            };

            var limitsOptions = new LimitsOptionsDbModel
            {
                CountMaxFriends = newSettings.CountMaxFriends,
                CountMinFriends = newSettings.CountMinFriends
            };
            
            var deleteFriendsOptions = new DeleteFriendsOptionsDbModel
            {
                DialogIsOver = new DialogIsOverModel
                {
                    IsEnabled = newSettings.EnableDialogIsOver,
                    Timer = newSettings.DialogIsOverTimer
                },
                IsAddedToGroupsAndPages = new IsAddedToGroupsAndPagesModel
                {
                    IsEnabled = newSettings.EnableIsAddedToGroupsAndPages,
                    Timer = newSettings.IsAddedToGroupsAndPagesTimer
                },
                IsWink = new IsWinkModel
                {
                    IsEnabled = newSettings.EnableIsWink,
                    Timer = newSettings.IsWinkTimer
                },
                IsWinkFriendsOfFriends = new IsWinkFriendsOfFriendsModel
                {
                    IsEnabled = newSettings.EnableIsWinkFriendsOfFriends,
                    Timer = newSettings.IsWinkFriendsOfFriendsTimer
                }
            };

            var command = new AddOrUpdateSettingsCommand
            {
                GroupId = newSettings.GroupId,
                GeoOptions = geoOptions,
                FriendsOptions = friendsOptions,
                MessageOptions = messageOptions,
                LimitsOptions = limitsOptions,
                CommunityOptions = communityOptions,
                DeleteFriendsOptions = deleteFriendsOptions
            };

            new AddOrUpdateSettingsCommandHandler(new DataBaseContext()).Handle(command);
            
            var accountsThisGroup =
                new GetAccountsByGroupSettingsIdQueryHandler(new DataBaseContext()).Handle(
                    new GetAccountsByGroupSettingsIdQuery
                    {
                        GroupSettingsId = newSettings.GroupId
                    });

            var accountsViewModel = accountsThisGroup.Select(model => new AccountViewModel
                {
                    Id = model.Id,
                    PageUrl = model.PageUrl,
                    Login = model.Login,
                    Password = model.Password,
                    FacebookId = model.FacebookId,
                    Proxy = model.Proxy,
                    ProxyLogin = model.ProxyLogin,
                    ProxyPassword = model.ProxyPassword,
                    Cookie = model.Cookie.CookieString,
                    Name = model.Name,
                    GroupSettingsId = model.GroupSettingsId,
                    AuthorizationDataIsFailed = model.AuthorizationDataIsFailed,
                    ProxyDataIsFailed = model.ProxyDataIsFailed,
                    IsDeleted = model.IsDeleted
                }).ToList();

            var modelNewData = GetNewGroupsAndPages(communityOptions, oldSettings);

            foreach (var accountModel in accountsViewModel)
            {
                if (modelNewData != null)
                {
                    if(modelNewData.Groups.Count != 0 || modelNewData.Pages.Count !=0)
                    {
                        new SaveNewSettingsCommandHandler(new DataBaseContext()).Handle(new SaveNewSettingsCommand
                        {
                            AccountId = accountModel.Id,
                            CommunityOptions = modelNewData,
                            GroupId = newSettings.GroupId
                        });
                    }
                }
                
                var model = accountModel;
                var updater = new Task(() => UpdateJobsTask(backgroundJobService, model, newSettings, oldSettings));
                updater.Start();
            }
        }

        private static void UpdateJobsTask(IBackgroundJobService backgroundJobService, AccountViewModel account, GroupSettingsViewModel newSettings, GroupSettingsViewModel oldSettings)
        {
            var model = new AddOrUpdateAccountModel
            {
                Account = account,
                NewSettings = newSettings,
                OldSettings = oldSettings
            };

            backgroundJobService.AddOrUpdateAccountJobs(model);
        }

        private static string ConvertListToString(IEnumerable<string> arrayString)
        {
            if (arrayString == null)
            {
                return string.Empty;
            }

            return arrayString.Aggregate("", (current, element) => current + (element + "\n"));
        }

        private static List<string> ConvertStringToList(string inputString)
        {
            if (inputString == null || inputString.Equals(string.Empty))
            {
                return new List<string>();
            }
            var splitPattern = inputString.Contains("\r\n") ? "\r\n" : "\n";

            var splitArray = inputString.Split(new string[] { splitPattern }, StringSplitOptions.None).ToList();

            var notEmptyElement = splitArray.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

            return notEmptyElement;
        }

        private static CommunityOptionsDbModel GetNewGroupsAndPages(CommunityOptionsDbModel newCommunityOptions, GroupSettingsViewModel oldSettings)
        {
            var oldCommunityOptions = new CommunityOptionsDbModel
            {
                Pages = oldSettings == null ? new List<string>() : ConvertStringToList(oldSettings.FacebookPages), // если опции не были созданы
                Groups = oldSettings == null ? new List<string>() : ConvertStringToList(oldSettings.FacebookGroups) 
            };

            var newGroups = new List<string>();
            var newPages = new List<string>();

            foreach (var group in newCommunityOptions.Groups)
            {
                if (group.Equals(string.Empty))
                {
                    continue;
                }

                if (oldCommunityOptions.Groups.Contains(group))
                {
                    continue;
                }

                newGroups.Add(group);
            }

            foreach (var page in newCommunityOptions.Pages)
            {
                if (page.Equals(string.Empty))
                {
                    continue;
                }

                if (oldCommunityOptions.Pages.Contains(page))
                {
                    continue;
                }

                newPages.Add(page);
            }

            return new CommunityOptionsDbModel
            {
                Groups = newGroups,
                Pages = newPages
            };
        }
    }
}
