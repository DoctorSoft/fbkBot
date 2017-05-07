using Constants.FunctionEnums;

namespace DataBase.QueriesAndCommands.Queries.JobQueue
{
    public class CheckJobQueueByAccountIdAndFunctionNameQuery : IQuery<bool>
    {
        public long AccountId { get; set; }

        public bool IsForSpy { get; set; }

        public FunctionName FunctionName { get; set; }
        
    }
}
