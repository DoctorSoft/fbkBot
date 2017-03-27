using System;
using Constants.FunctionEnums;

namespace DataBase.QueriesAndCommands.Models
{
    public class ScheduleRemovalOfFriendsModel
    {
        public long Id { get; set; }

        public long AccountId { get; set; }

        public long FriendId { get; set; }

        public bool ToDelete { get; set; }
        
        public FunctionName FunctionName { get; set; }

        public DateTime AddDateTime { get; set; }
    }
}
