using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.JobState.JobStateIsExist
{
    public class JobStateIsExistQueryHandler : IQueryHandler<JobStateIsExistQuery, bool>
    {
        private readonly DataBaseContext _context;

        public JobStateIsExistQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public bool Handle(JobStateIsExistQuery command)
        {
            var jobStatus = command.FriendId != null
                ? _context.JobsState.FirstOrDefault(
                    model =>
                        model.FunctionName == command.FunctionName 
                        && model.AccountId == command.AccountId 
                        && model.IsForSpy == command.IsForSpy
                        && model.FriendId == command.FriendId)
                : _context.JobsState.FirstOrDefault(
                    model => model.FunctionName == command.FunctionName
                        && model.IsForSpy == command.IsForSpy
                        && model.AccountId == command.AccountId);

            return jobStatus != null;
        }
    }
}
