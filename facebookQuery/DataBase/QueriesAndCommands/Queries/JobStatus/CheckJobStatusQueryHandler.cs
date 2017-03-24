using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.JobStatus
{
    public class CheckJobStatusQueryHandler : IQueryHandler<CheckJobStatusQuery, bool>
    {
        private readonly DataBaseContext _context;

        public CheckJobStatusQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public bool Handle(CheckJobStatusQuery command)
        {
            var jobStatus = command.FriendId != null
                ? _context.JobStatus.FirstOrDefault(
                    model =>
                        model.FunctionName == command.FunctionName && model.AccountId == command.AccountId &&
                        model.FriendId == command.FriendId)
                : _context.JobStatus.FirstOrDefault(
                    model => model.FunctionName == command.FunctionName && model.AccountId == command.AccountId);


            return jobStatus != null;
        }
    }
}
