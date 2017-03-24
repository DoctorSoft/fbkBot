using System;
using Constants.FunctionEnums;

namespace DataBase.QueriesAndCommands.Commands.ScheduleDeleteFriends
{
    public class AddScheduleCommand : ICommand<long>
    {
        public long AccountId { get; set; }

        public long FriendId { get; set; }

        public FunctionName FunctionName { get; set; }

        public DateTime AddedDateTime { get; set; }
    }
}
