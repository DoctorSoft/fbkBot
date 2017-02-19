using System.Collections.Generic;
using System.Linq;
using Constants.FunctionEnums;
using DataBase.QueriesAndCommands.Commands.JobQueue.AddQueue;
using DataBase.QueriesAndCommands.Commands.JobQueue.DeleteQueue;
using DataBase.QueriesAndCommands.Queries.Account.JobQueue.AddQueue;
using Services.ViewModels.QueueViewModels;

namespace Services.Services
{
    public class JobQueueService
    {
        public void AddToQueue(long accountId, FunctionName functionName)
        {
            new AddToQueueCommandHandler().Handle(new AddToQueueCommand
            {
                AccountId = accountId,
                FunctionName = functionName
            });
        }

        public void RemoveQueue(long queueId)
        {
            new DeleteQueueCommandHandler().Handle(new DeleteQueueCommand
            {
                QueueId = queueId
            });
        }

        public List<QueueViewModel> GetQueues(long accountId, int countRecords)
        {
            var queueDbModels = new GetJobModelFromQueueCommandHandler().Handle(new GetJobModelFromQueueCommand
            {
                AccountId = accountId,
                CountRecords = countRecords
            });

            return queueDbModels.Select(model => new QueueViewModel
            {
                AccountId = model.AccountId,
                Id = model.Id,
                FunctionName = model.FunctionName,
                AddedDateTime = model.AddedDateTime
            }).ToList();
        }
    }
}
