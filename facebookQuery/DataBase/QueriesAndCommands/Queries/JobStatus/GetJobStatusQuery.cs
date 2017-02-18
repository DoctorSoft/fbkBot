using Constants.FunctionEnums;

namespace DataBase.QueriesAndCommands.Queries.JobStatus
{
    public class GetJobStatusQuery : IQuery<JobStatusData>
    {
        public FunctionName FunctionName { get; set; }
        
    }
}
