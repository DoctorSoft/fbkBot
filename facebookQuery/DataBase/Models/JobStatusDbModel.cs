using System;
using Constants.FunctionEnums;

namespace DataBase.Models
{
    public class JobStatusDbModel
    {
        public long Id { get; set; }

        public long AccountId { get; set; }

        public FunctionName FunctionName { get; set; }

        public DateTime LastLaunchDateTime { get; set; }
    }
}
