using System.Collections.Generic;
using System.Linq;
using DataBase.Context;
using DataBase.Models;
using DataBase.QueriesAndCommands;
using DataBase.QueriesAndCommands.Commands.ScheduleDeleteFriends;
using DataBase.QueriesAndCommands.Models;
using DataBase.QueriesAndCommands.Queries.ScheduleDeleteFriends;
using Services.ViewModels.ScheduleDeleteFriendsModels;

namespace Services.Services
{
    public class ScheduleDeleteFriendsSerice
    {
        public List<ScheduleDeleteFriendsModel> GetSchedules()
        {
            var schedules = new GetScheduleDeleteFriendsQueryHandler(new DataBaseContext()).Handle(new GetScheduleDeleteFriendsQuery());

            return schedules.Select(model => new ScheduleDeleteFriendsModel
            {
                FunctionName = model.FunctionName,
                AccountId = model.AccountId,
                FriendId = model.FriendId,
                AddDateTime = model.AddDateTime,
                Id = model.Id
            }).ToList();
        }

        public bool DeleteSchedules(List<ScheduleDeleteFriendsModel> schedules)
        {
            var schedulesList = schedules.Select(model => new ScheduleRemovalOfFriendsModel
            {
                FunctionName = model.FunctionName,
                AccountId = model.AccountId,
                FriendId = model.FriendId,
                AddDateTime = model.AddDateTime,
                Id = model.Id
            }).ToList();

            new DeleteScheduleByIdCommandHandler(new DataBaseContext()).Handle(new DeleteScheduleByIdCommand
            {
                Schedules = schedulesList
            });

            return true;
        }
    }
}
