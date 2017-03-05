using System.Collections.Generic;
using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Queries.Account.JobQueue.AddQueue;

namespace DataBase.QueriesAndCommands.Queries.Account.JobQueue.GetQueue.GetAllQueues
{
    public class GetAllQueuesCommandHandler : ICommandHandler<GetAllQueuesCommand, List<JobQueueModel>>
    {
        private readonly DataBaseContext _context;

        public GetAllQueuesCommandHandler()
        {
            _context = new DataBaseContext();
        }

        public List<JobQueueModel> Handle(GetAllQueuesCommand command)
        {
            var queues = _context.JobsQueue
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
