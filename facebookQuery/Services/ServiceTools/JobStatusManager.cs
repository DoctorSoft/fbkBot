using System.Collections.Generic;
using System.Linq;
using Constants.FunctionEnums;
using DataBase.Context;
using DataBase.QueriesAndCommands.Queries.Functions;
using DataBase.QueriesAndCommands.Queries.JobStatus.GetJobStatuses.GetJobStatusesByAccounAndFunctiontId;
using DataBase.QueriesAndCommands.Queries.JobStatus.GetJobStatuses.GetJobStatusesByAccountId;
using Services.ViewModels.JobStatusModels;

namespace Services.ServiceTools
{
    public class JobStatusManager
    {
        public List<JobStatusViewModel> GetJobStatusesByAccountAndFunctionId(long accountId, FunctionName functionName,
            bool forSpy)
        {
            var statusModels =
                new GetJobStatusesByAccounAndFunctiontIdCommandHandler(new DataBaseContext()).Handle(
                    new GetJobStatusesByAccounAndFunctiontIdCommand
                    {
                        AccountId = accountId,
                        IsForSpy = forSpy,
                        FunctionName = functionName
                    });

            var result = statusModels.Select(model =>
                new JobStatusViewModel
                {
                    AccountId = model.AccountId,
                    AddDateTime = model.AddDateTime,
                    FriendId = model.FriendId,
                    FunctionName = model.FunctionName,
                    Id = model.Id,
                    IsForSpy = model.IsForSpy,
                    JobId = model.JobId
                }
                ).ToList();

            return result;
        }

        public List<JobStatusViewModel> GetAllJobStatusesByAccountId(long accountId, bool forSpy)
        {
            var functions = new GetFunctionsQueryHandler(new DataBaseContext()).Handle(new GetFunctionsQuery
            {
                ForSpy = false
            });

            var statusModels =
                new GetJobStatusesByAccountIdCommandHandler(new DataBaseContext()).Handle(
                    new GetJobStatusesByAccountIdCommand
                    {
                        AccountId = accountId,
                        IsForSpy = forSpy
                    });

            var result = statusModels.Select(model =>
            {
                var function = functions.FirstOrDefault(data => data.FunctionName == model.FunctionName);
                return function != null ? 
                    new JobStatusViewModel
                    {
                        AccountId = model.AccountId,
                        AddDateTime = model.AddDateTime,
                        FriendId = model.FriendId,
                        FunctionName = model.FunctionName,
                        FunctioStringnName = function.Name,
                        Id = model.Id,
                        IsForSpy = model.IsForSpy,
                        JobId = model.JobId
                    } : null;
            }).ToList();

            return result;
        }
    }
}
