using System.Collections.Generic;
using Constants.FunctionEnums;

namespace DataBase.QueriesAndCommands.Queries.JobStatus.GetJobStatuses.GetJobStatusesByAccounAndFunctiontId
{
    public class GetJobStatusesByAccounAndFunctiontIdCommand : ICommand<List<JobStatusModel>>
    {
        public long AccountId { get; set; }

        public FunctionName FunctionName { get; set; }

        public bool IsForSpy { get; set; }
    }
}
