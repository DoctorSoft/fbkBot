using Constants.FunctionEnums;

namespace DataBase.QueriesAndCommands.Queries.JobStatus
{
    public class CheckJobStatusQuery : IQuery<bool>
    {
        public long AccountId { get; set; }

        public FunctionName FunctionName { get; set; }
    }
}
