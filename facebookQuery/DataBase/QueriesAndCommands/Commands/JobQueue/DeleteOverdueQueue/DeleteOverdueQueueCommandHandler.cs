using System;
using System.Linq;
using System.Threading;
using DataBase.Context;
using DataBase.Models;

namespace DataBase.QueriesAndCommands.Commands.JobQueue.DeleteOverdueQueue
{
    public class DeleteOverdueQueueCommandHandler : ICommandHandler<DeleteOverdueQueueCommand, DeleteOverdueQueueResponseModel>
    {
        private readonly DataBaseContext _context;

        public DeleteOverdueQueueCommandHandler()
        {
            _context = new DataBaseContext();
        }

        public DeleteOverdueQueueResponseModel Handle(DeleteOverdueQueueCommand command)
        {
            var overdueMin = command.OverdueMin;
            var counter = 0;

            var queueModels = _context.JobsQueue.Select(model => new 
            {
                AccountId = model.AccountId,
                AddedDateTime = model.AddedDateTime,
                FriendId = model.FriendId,
                FunctionName = model.FunctionName,
                Id = model.Id,
                IsForSpy = model.IsForSpy,
                IsProcessed = model.IsProcessed
            }).AsEnumerable().Select(model => new JobQueueDbModel
            {
                AccountId = model.AccountId,
                AddedDateTime = model.AddedDateTime,
                FriendId = model.FriendId,
                FunctionName = model.FunctionName,
                Id = model.Id,
                IsForSpy = model.IsForSpy,
                IsProcessed = model.IsProcessed
            }).ToList();

            if (queueModels.Count == 0)
            {
                return new DeleteOverdueQueueResponseModel();
            }

            foreach (var jobQueueDbModel in queueModels)
            {
                var isOld = CheckOverdue(jobQueueDbModel.AddedDateTime) > overdueMin;
                if (!isOld)
                {
                    continue;
                }

                var customer = _context.JobsQueue.Single(o => o.Id == jobQueueDbModel.Id);
                _context.JobsQueue.Remove(customer);
                _context.SaveChanges();

                Thread.Sleep(1000);
                counter++;
            }

            
            return new DeleteOverdueQueueResponseModel
            {
                CountRemove = counter
            };
        }

        public long CheckOverdue(DateTime addedDateTime)
        {
            var differenceTime = DateTime.Now - addedDateTime;

            var differenceMin = differenceTime.Hours*60 + differenceTime.Minutes;

            return differenceMin;
        }
    }
}
