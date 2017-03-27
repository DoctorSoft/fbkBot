using System;
using Constants.FunctionEnums;

namespace Services.ViewModels.ScheduleDeleteFriendsModels
{
    public class ScheduleDeleteFriendsModel
    {
        public long Id { get; set; }

        public long AccountId { get; set; }

        public long FriendId { get; set; }

        public bool ToDelete { get; set; }

        public FunctionName FunctionName { get; set; }

        public DateTime AddDateTime { get; set; }
    }
}
