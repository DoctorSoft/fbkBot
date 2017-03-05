using Constants.FunctionEnums;

namespace DataBase.QueriesAndCommands.Queries.JobStatus
{
    public class GetJobStatusQuery : IQuery<JobStatusData>
    {
        public long AccountId { get; set; }

        public FunctionName FunctionName { get; set; }
        
    }
}
