using System.Collections.Generic;

namespace DataBase.QueriesAndCommands.Queries.JobStatus.GetJobStatuses.GetJobStatusesByAccountId
{
    public class GetJobStatusesByAccountIdCommand : ICommand<List<JobStatusModel>>
    {
        public long AccountId { get; set; }

        public bool IsForSpy { get; set; }
    }
}
