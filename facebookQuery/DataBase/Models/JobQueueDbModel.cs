using System;
using Constants.FunctionEnums;

namespace DataBase.Models
{
    public class JobQueueDbModel
    {
        public long Id { get; set; }

        public long AccountId { get; set; }

        public long? FriendId { get; set; }

        public FunctionName FunctionName { get; set; }

        public DateTime AddedDateTime { get; set; }

        public bool IsProcessed { get; set; }

        public bool IsForSpy { get; set; }
    }
}
