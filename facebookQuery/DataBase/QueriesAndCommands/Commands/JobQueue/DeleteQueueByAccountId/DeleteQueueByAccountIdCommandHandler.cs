using System;
using System.Collections.Generic;
using System.Linq;
using Constants.FunctionEnums;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Commands.JobQueue.DeleteQueueByAccountId
{
    public class DeleteQueueByAccountIdCommandHandler : ICommandHandler<DeleteQueueByAccountIdCommand, List<string>>
    {
        private readonly DataBaseContext _context;

        public DeleteQueueByAccountIdCommandHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public List<string> Handle(DeleteQueueByAccountIdCommand command)
        {
            List<string> resultList;

            var jobQueues = _context.JobsQueue
                .Where(model => model.AccountId == command.AccountId && model.IsForSpy == command.IsForSpy)
                .Where(model => model.FunctionName != FunctionName.RefreshCookies);

            if (command.FunctionName != 0)
            {
                jobQueues = jobQueues.Where(model => model.FunctionName == command.FunctionName);
            }
            try
            {
                resultList = jobQueues.Select(model => model.JobId).ToList();
                _context.JobsQueue.RemoveRange(jobQueues);

                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }

            return resultList;
        }
    }
}
