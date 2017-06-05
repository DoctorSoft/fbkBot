using System;
using System.Collections.Generic;
using System.Linq;
using Constants.FunctionEnums;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Commands.JobStatus.DeleteJobStatusesByAccountId
{
    public class DeleteJobStatusesByAccountIdCommandHandler : ICommandHandler<DeleteJobStatusesByAccountIdCommand, List<string>>
    {
        private readonly DataBaseContext _context;

        public DeleteJobStatusesByAccountIdCommandHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public List<string> Handle(DeleteJobStatusesByAccountIdCommand command)
        {
            List<string> resultList;

            var jobStatuses = _context.JobStatus
                .Where(model => model.AccountId == command.AccountId && model.IsForSpy == command.IsForSpy)
                .Where(model => model.FunctionName != FunctionName.RefreshCookies);

            if (command.FunctionName != 0)
            {
                jobStatuses = jobStatuses.Where(model => model.FunctionName == command.FunctionName);
            }
            try
            {
                resultList = jobStatuses.Select(model => model.JobId).ToList();
                _context.JobStatus.RemoveRange(jobStatuses);

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
