using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.JobQueue
{
    public class CheckJobQueueByAccountIdAndFunctionNameQueryHandler : IQueryHandler<CheckJobQueueByAccountIdAndFunctionNameQuery, bool>
    {
        private readonly DataBaseContext _context;

        public CheckJobQueueByAccountIdAndFunctionNameQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public bool Handle(CheckJobQueueByAccountIdAndFunctionNameQuery command)
        {
            var jobStatus = _context.JobsQueue
                .FirstOrDefault(model => model.FunctionName == command.FunctionName 
                    && model.AccountId == command.AccountId 
                    && model.IsForSpy == command.IsForSpy);

            return jobStatus != null;
        }
    }
}
