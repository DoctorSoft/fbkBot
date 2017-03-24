using System.Collections.Generic;
using Constants.FunctionEnums;

namespace DataBase.QueriesAndCommands.Queries.JobStatus
{
    public class GetJobStatusQuery : IQuery<List<JobStatusModel>>
    {
        public long AccountId { get; set; }

        public FunctionName FunctionName { get; set; }

        public long? FriendId { get; set; }
    }
}
