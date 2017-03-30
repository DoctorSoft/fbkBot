using System.Collections.Generic;
using Constants.FunctionEnums;

namespace DataBase.QueriesAndCommands.Queries.Account.JobQueue.GetQueue.GetQueuesByAccountId
{
    public class GetJobQueuesByAccountIdCommand : ICommand<List<JobQueueModel>>
    {
        public long AccountId { get; set; }

        public FunctionName? FunctionName { get; set; }
    }
}
