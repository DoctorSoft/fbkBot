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

        public TimeSpan LaunchDateTime { get; set; }

        public DateTime AddDateTime { get; set; }
    }
}
