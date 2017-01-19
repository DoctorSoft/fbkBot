using System.Linq;
using DataBase.Context;
using DataBase.Migrations;
using DataBase.QueriesAndCommands.Commands.Groups;
using DataBase.QueriesAndCommands.Commands.Settings;
using DataBase.QueriesAndCommands.Queries.Account;
using DataBase.QueriesAndCommands.Queries.Groups;
using DataBase.QueriesAndCommands.Queries.Settings;
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
            var settings = new GetSettingsByGroupSettingsIdHandler(new DataBaseContext()).Handle(new GetSettingsByGroupSettingsIdQuery
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
                LivesPlace = settings.LivesPlace,
                SchoolPlace = settings.SchoolPlace,
                WorkPlace = settings.WorkPlace,
                DelayTimeSendNewFriend = settings.DelayTimeSendNewFriend,
                DelayTimeSendUnanswered = settings.DelayTimeSendUnanswered,
                DelayTimeSendUnread = settings.DelayTimeSendUnread,
                UnansweredDelay = settings.UnansweredDelay
            };
        }

        public void UpdateSettings(GroupSettingsViewModel newSettings, IJobService jobService)
        {
            new AddOrUpdateSettingsCommandHandler(new DataBaseContext()).Handle(new AddOrUpdateSettingsCommand
            {
                Gender = newSettings.Gender,
                GroupId = newSettings.GroupId,
                LivesPlace = newSettings.LivesPlace,
                SchoolPlace = newSettings.SchoolPlace,
                WorkPlace = newSettings.WorkPlace,
                DelayTimeSendNewFriend = newSettings.DelayTimeSendNewFriend,
                DelayTimeSendUnanswered = newSettings.DelayTimeSendUnanswered,
                DelayTimeSendUnread = newSettings.DelayTimeSendUnread,
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
    }
}
