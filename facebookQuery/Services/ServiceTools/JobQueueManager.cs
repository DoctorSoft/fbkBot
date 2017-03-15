using Constants.FunctionEnums;
using DataBase.Context;
using DataBase.QueriesAndCommands.Queries.JobQueue;

namespace Services.ServiceTools
{
    public class JobQueueManager
    {
        public bool IsInTheQueue(FunctionName functionName, long accountId)
        {
            var status =
                new CheckJobQueueByAccountIdAndFunctionNameQueryHandler(new DataBaseContext()).Handle(
                    new CheckJobQueueByAccountIdAndFunctionNameQuery
                    {
                        AccountId = accountId,
                        FunctionName = functionName
                    });

            return status;
        }
    }
}
