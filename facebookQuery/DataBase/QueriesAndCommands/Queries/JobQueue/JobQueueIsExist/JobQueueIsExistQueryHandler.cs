using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.JobQueue.JobQueueIsExist
{
    public class JobQueueIsExistQueryHandler : IQueryHandler<JobQueueIsExistQuery, bool>
    {
        private readonly DataBaseContext _context;

        public JobQueueIsExistQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public bool Handle(JobQueueIsExistQuery command)
        {
            var jobStatus = command.FriendId != null
                ? _context.JobsQueue.FirstOrDefault(
                    model =>
                        model.FunctionName == command.FunctionName 
                        && model.AccountId == command.AccountId 
                        && model.IsForSpy == command.IsForSpy
                        && model.FriendId == command.FriendId)
                : _context.JobsQueue.FirstOrDefault(
                    model => model.FunctionName == command.FunctionName
                        && model.IsForSpy == command.IsForSpy
                        && model.AccountId == command.AccountId);

            return jobStatus != null;
        }
    }
}
