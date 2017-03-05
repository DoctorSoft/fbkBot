using System.Collections.Generic;
using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Queries.Account.JobQueue.AddQueue;

namespace DataBase.QueriesAndCommands.Queries.Account.JobQueue.GetQueue.GetQueuesByAccountId
{
    public class GetJobQueuesByAccountIdCommandHandler : ICommandHandler<GetJobQueuesByAccountIdCommand, List<JobQueueModel>>
    {
        private readonly DataBaseContext _context;

        public GetJobQueuesByAccountIdCommandHandler()
        {
            _context = new DataBaseContext();
        }

        public List<JobQueueModel> Handle(GetJobQueuesByAccountIdCommand command)
        {
            var queues = _context.JobsQueue
                .Where(model=>model.AccountId == command.AccountId)
                .OrderByDescending(model => model.AddedDateTime)
                .Select(model => new JobQueueModel
                {
                    AccountId = model.AccountId,
                    Id = model.Id,
                    AddedDateTime = model.AddedDateTime,
                    FunctionName = model.FunctionName
                }).ToList();

            return queues;
        }
    }
}
