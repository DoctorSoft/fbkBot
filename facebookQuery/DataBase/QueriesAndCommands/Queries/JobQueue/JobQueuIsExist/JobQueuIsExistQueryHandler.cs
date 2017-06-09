using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.JobQueue.JobQueuIsExist
{
    public class JobQueuIsExistQueryHandler : IQueryHandler<JobQueuIsExistQuery, bool>
    {
        private readonly DataBaseContext _context;

        public JobQueuIsExistQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public bool Handle(JobQueuIsExistQuery command)
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
