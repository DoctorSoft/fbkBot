using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using CommonModels;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.Groups;
using DataBase.QueriesAndCommands.Commands.Settings;
using DataBase.QueriesAndCommands.Models.JsonModels;
using DataBase.QueriesAndCommands.Queries.Account;
using DataBase.QueriesAndCommands.Queries.Groups;
using DataBase.QueriesAndCommands.Queries.Settings;
using Newtonsoft.Json;
using Services.Interfaces;
using Services.ServiceTools;
using Services.ViewModels.GroupModels;
using Services.ViewModels.HomeModels;

namespace Services.Services
{
    public class GroupService
    {
        private readonly AccountSettingsManager _accountManager;

        public GroupService()
        {
            _accountManager = new AccountSettingsManager();
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
                CountMaxFriends = settings.LimitsOptions.CountMaxFriends,
                CountMinFriends = settings.LimitsOptions.CountMinFriends,
            };
        }

        public void UpdateSettings(GroupSettingsViewModel newSettings, IBackgroundJobService backgroundJobService)
        {
            var oldSettings = _accountManager.GetSettings(newSettings.GroupId);

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

            var command = new AddOrUpdateSettingsCommand
            {
                GroupId = newSettings.GroupId,
                GeoOptions = geoOptions,
                FriendsOptions = friendsOptions,
                MessageOptions = messageOptions,
                LimitsOptions = limitsOptions
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

            foreach (var accountModel in accountsViewModel)
            {
                var updater = new Task(() => UpdateJobsTask(backgroundJobService, accountModel, newSettings, oldSettings));
                updater.Start();
            }
        }

        private static void UpdateJobsTask(IBackgroundJobService backgroundJobService, AccountViewModel account, GroupSettingsViewModel newSettings, GroupSettingsViewModel oldSettings)
        {
            backgroundJobService.AddOrUpdateAccountJobs(account, newSettings, oldSettings);
        }
    }
}
