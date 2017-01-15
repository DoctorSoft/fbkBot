using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.Groups;
using DataBase.QueriesAndCommands.Commands.Settings;
using DataBase.QueriesAndCommands.Queries.Groups;
using DataBase.QueriesAndCommands.Queries.Settings;
using Services.ViewModels.GroupModels;

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
                DelayTimeSendUnread = settings.DelayTimeSendUnread
            };
        }

        public void UpdateSettings(GroupSettingsViewModel newSettings)
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
                DelayTimeSendUnread = newSettings.DelayTimeSendUnread
            });
        }
    }
}
