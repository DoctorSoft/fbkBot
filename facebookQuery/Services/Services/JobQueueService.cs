using System.Collections.Generic;
using System.Linq;
using Constants.FunctionEnums;
using DataBase.QueriesAndCommands.Commands.JobQueue.AddQueue;
using DataBase.QueriesAndCommands.Commands.JobQueue.DeleteQueue;
using DataBase.QueriesAndCommands.Queries.Account.JobQueue.AddQueue;
using DataBase.QueriesAndCommands.Queries.Account.JobQueue.GetQueue.GetAllQueues;
using DataBase.QueriesAndCommands.Queries.Account.JobQueue.GetQueue.GetQueuesByAccountId;
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

        public List<QueueViewModel> GetQueuesByAccountId(long accountId)
        {
            var queuesDbModels = new GetJobQueuesByAccountIdCommandHandler().Handle(new GetJobQueuesByAccountIdCommand
            {
                AccountId = accountId
            });

            return queuesDbModels.Select(model => new QueueViewModel
            {
                AccountId = model.AccountId,
                Id = model.Id,
                FunctionName = model.FunctionName,
                AddedDateTime = model.AddedDateTime
            }).ToList();
        }

        public List<QueueViewModel> GetQueuesByAccountAndFunctionName(long accountId, FunctionName functionName)
        {
            var queuesDbModels = new GetJobQueuesByAccountIdCommandHandler().Handle(new GetJobQueuesByAccountIdCommand
            {
                AccountId = accountId,
                FunctionName = functionName
            });

            return queuesDbModels.Select(model => new QueueViewModel
            {
                AccountId = model.AccountId,
                Id = model.Id,
                FunctionName = model.FunctionName,
                AddedDateTime = model.AddedDateTime
            }).ToList();
        }

        public List<QueueViewModel> GetAllQueues()
        {
            var queuesDbModels = new GetAllQueuesCommandHandler().Handle(new GetAllQueuesCommand());

            return queuesDbModels.Select(model => new QueueViewModel
            {
                AccountId = model.AccountId,
                Id = model.Id,
                FunctionName = model.FunctionName,
                AddedDateTime = model.AddedDateTime
            }).ToList();
        }

        public List<List<QueueViewModel>> GetGroupedQueue()
        {
            var queuesDbModels = GetAllQueues();

            var groupedList = queuesDbModels.GroupBy(model => model.AccountId).Select(grp => grp.ToList()).ToList(); 

            return groupedList;
        }
    }
}
