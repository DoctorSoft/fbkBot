using System;
using Constants.FunctionEnums;

namespace DataBase.QueriesAndCommands.Queries.JobStatus
{
    public class JobStatusData
    {
        public long Id { get; set; }

        public long AccountId { get; set; }

        public FunctionName FunctionName { get; set; }

        public DateTime LastLaunchDateTime { get; set; }
    }
}
