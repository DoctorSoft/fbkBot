using System;
using Constants.FunctionEnums;

namespace DataBase.QueriesAndCommands.Queries.Account.JobQueue.GetQueue
{
    public class JobQueueModel
    {
        public long Id { get; set; }

        public long AccountId { get; set; }

        public long? FriendId { get; set; }

        public FunctionName FunctionName { get; set; }

        public DateTime AddedDateTime { get; set; }
    }
}
