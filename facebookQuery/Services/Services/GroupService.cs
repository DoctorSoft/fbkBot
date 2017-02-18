using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using Constants.UrlEnums;
using DataBase.Context;
using DataBase.Migrations;
using DataBase.QueriesAndCommands.Commands.Groups;
using DataBase.QueriesAndCommands.Commands.Settings;
using DataBase.QueriesAndCommands.Queries.Account;
using DataBase.QueriesAndCommands.Queries.Groups;
using DataBase.QueriesAndCommands.Queries.Settings;
using Newtonsoft.Json;
using Services.Interfaces;
using Services.ViewModels.GroupModels;
using Services.ViewModels.HomeModels;

namespace Services.Services
{
    public class GroupService
    {
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
                new GetSettingsByGroupSettingsIdHandler(new DataBaseContext()).Handle(new GetSettingsByGroupSettingsIdQuery
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
                Gender = settings.Gender,
                Cities = ConvertJsonToString(settings.Cities),
                Countries = ConvertJsonToString(settings.Countries),
                RetryTimeSendUnread = settings.RetryTimeSendUnread,
                RetryTimeConfirmFriendships = settings.RetryTimeConfirmFriendships,
                RetryTimeGetNewAndRecommendedFriends = settings.RetryTimeGetNewAndRecommendedFriends,
                RetryTimeRefreshFriends = settings.RetryTimeRefreshFriends,
                RetryTimeSendNewFriend = settings.RetryTimeSendNewFriend,
                RetryTimeSendRequestFriendships = settings.RetryTimeSendRequestFriendships,
                RetryTimeSendUnanswered = settings.RetryTimeSendUnanswered,
                UnansweredDelay = settings.UnansweredDelay
            };
        }

        public void UpdateSettings(GroupSettingsViewModel newSettings, IJobService jobService)
        {
            new AddOrUpdateSettingsCommandHandler(new DataBaseContext()).Handle(new AddOrUpdateSettingsCommand
            {
                Gender = newSettings.Gender,
                GroupId = newSettings.GroupId,
                Cities = ConvertToJson(newSettings.Cities),
                Countries = ConvertToJson(newSettings.Countries),
                RetryTimeSendUnread = newSettings.RetryTimeSendUnread,
                RetryTimeConfirmFriendships = newSettings.RetryTimeConfirmFriendships,
                RetryTimeGetNewAndRecommendedFriends = newSettings.RetryTimeGetNewAndRecommendedFriends,
                RetryTimeRefreshFriends = newSettings.RetryTimeRefreshFriends,
                RetryTimeSendNewFriend = newSettings.RetryTimeSendNewFriend,
                RetryTimeSendRequestFriendships = newSettings.RetryTimeSendRequestFriendships,
                RetryTimeSendUnanswered = newSettings.RetryTimeSendUnanswered,
                UnansweredDelay = newSettings.UnansweredDelay
            });

            var accountsToChangeJobs =
                new GetAccountsByGroupSettingsIdQueryHandler(new DataBaseContext()).Handle(
                    new GetAccountsByGroupSettingsIdQuery
                    {
                        GroupSettingsId = newSettings.GroupId
                    });

            foreach (var account in accountsToChangeJobs)
            {
                jobService.AddOrUpdateAccountJobs(new AccountViewModel
                {
                    Id = account.Id,
                    Name = account.Name,
                    PageUrl = account.PageUrl,
                    FacebookId = account.FacebookId,
                    Password = account.Password,
                    Login = account.Login,
                    Proxy = account.Proxy,
                    ProxyLogin = account.ProxyLogin,
                    ProxyPassword = account.ProxyPassword,
                    Cookie = account.Cookie.CookieString,
                    GroupSettingsId = account.GroupSettingsId
                });
            }
        }

        private string ConvertToJson(string startData)
        {
            try
            {
                var stringSeparators = new[] {"\r\n"};
                var lines = startData.Split(stringSeparators, StringSplitOptions.None);

                var cancelRequestFriendshipParameters = lines.Select(s => s).ToList();

                var js = new JavaScriptSerializer();
                var jsonWink = js.Serialize(cancelRequestFriendshipParameters.Select(pair => pair).ToList());

                return jsonWink;
            }
            catch (Exception ex)
            {
                return "";
            }
        }


        private string ConvertJsonToString(string jsonData)
        {
            try
            {
                var words = JsonConvert.DeserializeObject<List<string>>(jsonData);

                var result = string.Join("\r\n", words.Select(s => s));

                return result;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
