using System;
using Constants.FunctionEnums;

namespace DataBase.Models
{
    public class JobStatusDbModel
    {
        public long Id { get; set; }

        public long AccountId { get; set; }

        public string JobId { get; set; }

        public FunctionName FunctionName { get; set; }

        public long? FriendId { get; set; }

        public string LaunchDateTime { get; set; }

        public DateTime AddDateTime { get; set; }

        public bool IsForSpy { get; set; }
    }
}
