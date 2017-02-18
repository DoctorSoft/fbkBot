using System.Collections.Generic;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.Account.JobQueue.AddQueue
{
    public class GetJobModelFromQueueCommandHandler : ICommandHandler<GetJobModelFromQueueCommand, List<JobQueueModel>>
    {
        private readonly DataBaseContext _context;

        public GetJobModelFromQueueCommandHandler()
        {
            _context = new DataBaseContext();
        }

        public List<JobQueueModel> Handle(GetJobModelFromQueueCommand command)
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
                }).Take(command.CountRecords).ToList();

            return queues;
        }
    }
}
