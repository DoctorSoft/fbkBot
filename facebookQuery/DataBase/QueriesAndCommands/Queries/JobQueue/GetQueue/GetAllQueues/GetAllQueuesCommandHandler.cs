using System.Collections.Generic;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.JobQueue.GetQueue.GetAllQueues
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
                .Where(model => !model.IsProcessed) // которые не выполняются
                .Where(model => !_context.JobsQueue.Any(dbModel => dbModel.AccountId == model.AccountId && dbModel.IsProcessed)) //не берем аккаунты которые уже выполняются
                .Select(model => new JobQueueModel
                {
                    AccountId = model.AccountId,
                    Id = model.Id,
                    AddedDateTime = model.AddedDateTime,
                    FunctionName = model.FunctionName,
                    FriendId = model.FriendId,
                    IsForSpy = model.IsForSpy,
                    IsProcessed = model.IsProcessed,
                    LaunchDateTime = model.LaunchDateTime,
                    JobId = model.JobId
                }).ToList();

            return queues;
        }
    }
}
