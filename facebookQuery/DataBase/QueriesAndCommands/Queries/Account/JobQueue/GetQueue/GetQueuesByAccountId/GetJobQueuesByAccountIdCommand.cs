using System.Collections.Generic;
using Constants.FunctionEnums;
using DataBase.QueriesAndCommands.Queries.Account.JobQueue.AddQueue;

namespace DataBase.QueriesAndCommands.Queries.Account.JobQueue.GetQueue.GetQueuesByAccountId
{
    public class GetJobQueuesByAccountIdCommand : ICommand<List<JobQueueModel>>
    {
        public long AccountId { get; set; }

        public FunctionName? FunctionName { get; set; }
    }
}
