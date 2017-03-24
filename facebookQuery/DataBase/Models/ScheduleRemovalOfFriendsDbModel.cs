using System;
using Constants.FunctionEnums;

namespace DataBase.Models
{
    public class ScheduleRemovalOfFriendsDbModel
    {
        public long Id { get; set; }

        public long AccountId { get; set; }

        public long FriendId { get; set; }
        
        public FunctionName FunctionName { get; set; }

        public DateTime AddDateTime { get; set; }
    }
}
