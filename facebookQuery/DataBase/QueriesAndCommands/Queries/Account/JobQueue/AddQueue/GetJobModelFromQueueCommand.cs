using System.Collections.Generic;

namespace DataBase.QueriesAndCommands.Queries.Account.JobQueue.AddQueue
{
    public class GetJobModelFromQueueCommand : ICommand<List<JobQueueModel>>
    {
        public long AccountId { get; set; }

        public int CountRecords { get; set; }
    }
}
