using System.Collections.Generic;
using DataBase.QueriesAndCommands.Models;

namespace DataBase.QueriesAndCommands.Commands.ScheduleDeleteFriends
{
    public class DeleteScheduleByIdCommand : ICommand<VoidCommandResponse>
    {
        public List<ScheduleRemovalOfFriendsModel> Schedules { get; set; }
    }
}
