
using System;
using CommonModels;
using Constants.FunctionEnums;

namespace DataBase.QueriesAndCommands.Queries.JobStatus
{
    public class JobStatusModel
    {
        public long Id { get; set; }

        public long AccountId { get; set; }

        public string JobId { get; set; }

        public long? FriendId { get; set; }

        public FunctionName FunctionName { get; set; }

        public TimeModel LaunchTime { get; set; }

        public DateTime AddDateTime { get; set; }

        public bool IsForSpy { get; set; }
    }
}
