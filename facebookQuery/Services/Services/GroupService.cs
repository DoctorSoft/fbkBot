using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using CommonModels;
using DataBase.Context;
using DataBase.Models;
using DataBase.QueriesAndCommands.Commands.Groups;
using DataBase.QueriesAndCommands.Commands.NewSettings;
using DataBase.QueriesAndCommands.Commands.Settings;
using DataBase.QueriesAndCommands.Models.JsonModels;
using DataBase.QueriesAndCommands.Queries.Account;
using DataBase.QueriesAndCommands.Queries.Groups;
using DataBase.QueriesAndCommands.Queries.NewSettings;
using DataBase.QueriesAndCommands.Queries.Settings;
using Newtonsoft.Json;
using OpenQA.Selenium.Edge;
using Services.Interfaces;
using Services.ServiceTools;
using Services.ViewModels.GroupModels;
using Services.ViewModels.HomeModels;
using Services.ViewModels.NewSettingsModels;

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

        public void JoinTheNewGroup(AccountViewModel account)
        {
            
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
                FacebookGroups = ConvertListToString(settings.CommunityOptions.Groups),
                FacebookPages = ConvertListToString(settings.CommunityOptions.Pages),
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

            var communityOptions = new CommunityOptionsDbModel
            {
                Groups = ConvertStringToList(newSettings.FacebookGroups),
                Pages = ConvertStringToList(newSettings.FacebookPages)
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

            var command = new AddOrUpdateSettingsCommand
            {
                GroupId = newSettings.GroupId,
                GeoOptions = geoOptions,
                FriendsOptions = friendsOptions,
                MessageOptions = messageOptions,
                LimitsOptions = limitsOptions,
                CommunityOptions = communityOptions
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
            backgroundJobService.AddOrUpdateAccountJobs(account, newSettings, oldSettings);
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
            if (inputString.Equals(string.Empty))
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
                Pages = ConvertStringToList(oldSettings.FacebookPages),
                Groups = ConvertStringToList(oldSettings.FacebookGroups)
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
