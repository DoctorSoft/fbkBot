using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.Groups;
using DataBase.QueriesAndCommands.Queries.Groups;
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
    }
}
