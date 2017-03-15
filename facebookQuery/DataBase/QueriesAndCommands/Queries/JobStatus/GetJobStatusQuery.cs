using Constants.FunctionEnums;

namespace DataBase.QueriesAndCommands.Queries.JobStatus
{
    public class GetJobStatusQuery : IQuery<JobStatusModel>
    {
        public long AccountId { get; set; }

        public FunctionName FunctionName { get; set; }
    }
}
