using System.Collections.Generic;
using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.JobQueue.AddQueue;
using DataBase.QueriesAndCommands.Commands.JobQueue.DeleteOverdueQueue;
using DataBase.QueriesAndCommands.Commands.JobQueue.DeleteQueue;
using DataBase.QueriesAndCommands.Commands.JobQueue.DeleteQueueByAccountId;
using DataBase.QueriesAndCommands.Commands.JobQueue.MarkProcessedStatus;
using DataBase.QueriesAndCommands.Queries.Functions.GetFunctionName;
using DataBase.QueriesAndCommands.Queries.JobQueue.GetQueue.GetAllQueues;
using DataBase.QueriesAndCommands.Queries.JobQueue.GetQueue.GetQueuesByAccountId;
using DataBase.QueriesAndCommands.Queries.JobQueue.JobQueueIsExist;
using Services.ViewModels.QueueViewModels;

namespace Services.Services
{
    public class JobQueueService
    {
        public void AddToQueue(JobQueueViewModel model)
        {
            new AddToQueueCommandHandler().Handle(new AddToQueueCommand
            {
                AccountId = model.AccountId,
                FunctionName = model.FunctionName,
                IsUnique = true,
                FriendId = model.FriendId,
                IsForSpy = model.IsForSpy,
                JobId = model.JobId,
                LaunchDateTime = model.LaunchDateTime
            });
        }
        
        public void RemoveQueue(long queueId)
        {
            new DeleteQueueCommandHandler().Handle(new DeleteQueueCommand
            {
                QueueId = queueId
            });
        }

        public long RemoveOverdueQueue(int overdue)
        {
            var removeQueuesModel = new DeleteOverdueQueueCommandHandler().Handle(new DeleteOverdueQueueCommand
            {
                OverdueMin = overdue
            });

            return removeQueuesModel.CountRemove;
        }

        public void MarkIsProcessedQueue(JobQueueViewModel model)
        {
            new MarkProcessedStatusCommandHandler().Handle(new MarkProcessedStatusCommand
            {
                AccountId = model.AccountId,
                FunctionName = model.FunctionName,
                FriendId = model.FriendId,
                IsForSpy = model.IsForSpy
            });
        }

        public List<JobQueueViewModel> GetQueuesByAccountId(JobQueueViewModel model)
        {
            var queuesDbModels = new GetJobQueuesByAccountIdCommandHandler().Handle(new GetJobQueuesByAccountIdCommand
            {
                AccountId = model.AccountId,
                IsForSpy = model.IsForSpy
            });

            return queuesDbModels.Select(queueModel => new JobQueueViewModel
            {
                AccountId = queueModel.AccountId,
                Id = queueModel.Id,
                FunctionName = queueModel.FunctionName,
                AddedDateTime = queueModel.AddedDateTime,
                IsForSpy = queueModel.IsForSpy,
                FriendId = queueModel.FriendId,
                IsProcessed = queueModel.IsProcessed
            }).ToList();
        }

        public List<JobQueueViewModel> GetQueuesByAccountAndFunctionName(JobQueueViewModel model)
        {
            var queuesDbModels = new GetJobQueuesByAccountIdCommandHandler().Handle(new GetJobQueuesByAccountIdCommand
            {
                AccountId = model.AccountId,
                IsForSpy = model.IsForSpy,
                FunctionName = model.FunctionName
            });

            return queuesDbModels.Select(queueModel => new JobQueueViewModel
            {
                AccountId = queueModel.AccountId,
                Id = queueModel.Id,
                FunctionName = queueModel.FunctionName,
                AddedDateTime = queueModel.AddedDateTime,
                IsForSpy = queueModel.IsForSpy,
                FriendId = queueModel.FriendId,
                FunctionStringName = new GetFunctionNameByNameQueryHandler(new DataBaseContext()).Handle(new GetFunctionNameByNameQuery
                {
                    FunctionName = queueModel.FunctionName
                }),
                JobId = queueModel.JobId,
                IsProcessed = queueModel.IsProcessed
            }).ToList();
        }

        public List<JobQueueViewModel> GetAllQueues()
        {
            var queuesDbModels = new GetAllQueuesCommandHandler().Handle(new GetAllQueuesCommand());

            return queuesDbModels.Select(model => new JobQueueViewModel
            {
                AccountId = model.AccountId,
                Id = model.Id,
                FunctionName = model.FunctionName,
                AddedDateTime = model.AddedDateTime,
                FriendId = model.FriendId,
                IsForSpy = model.IsForSpy,
                IsProcessed = model.IsProcessed,
                JobId = model.JobId
            }).ToList();
        }

        public List<List<JobQueueViewModel>> GetGroupedQueue()
        {
            var queuesDbModels = GetAllQueues();

            var groupedList = queuesDbModels.GroupBy(model => new {model.AccountId, model.IsForSpy}).Select(grp => grp.ToList()).ToList(); 

            return groupedList;
        }

        public List<string> DeleteJobQueuesByAccountId(JobQueueViewModel model)
        {
            var jobsId = new DeleteQueueByAccountIdCommandHandler(new DataBaseContext()).Handle(new DeleteQueueByAccountIdCommand
            {
                AccountId = model.AccountId,
                IsForSpy = model.IsForSpy,
                FunctionName = model.FunctionName
            });

            return jobsId;
        }

        public bool JobIsRun(JobQueueViewModel model)
        {
            var jobIsExist = new JobQueueIsExistQueryHandler(new DataBaseContext()).Handle(new JobQueueIsExistQuery
            {
                AccountId = model.AccountId,
                FunctionName = model.FunctionName,
                IsForSpy = model.IsForSpy,
                FriendId = model.FriendId
            });

            return jobIsExist;
        }
    }
}
