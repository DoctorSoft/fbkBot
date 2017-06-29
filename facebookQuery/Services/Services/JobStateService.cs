using System.Collections.Generic;
using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.JobState.AddState;
using DataBase.QueriesAndCommands.Commands.JobState.DeleteState;
using DataBase.QueriesAndCommands.Queries.JobState.GetState.GetStatesByModel;
using DataBase.QueriesAndCommands.Queries.JobState.JobStateIsExist;
using Hangfire.Common;
using Services.ViewModels.JobStateViewModels;

namespace Services.Services
{
    public class JobStateService
    {
        public bool CheckExist(JobStateViewModel jobState)
        {
            return new JobStateIsExistQueryHandler(new DataBaseContext()).Handle(new JobStateIsExistQuery
            {
                AccountId = jobState.AccountId,
                FunctionName = jobState.FunctionName,
                IsForSpy = jobState.IsForSpy,
                FriendId = jobState.FriendId
            });
        }

        public void AddJobState(JobStateViewModel jobState)
        {
            new AddJobStateCommandHandler().Handle(new AddJobStateCommand
            {
                AccountId = jobState.AccountId,
                FunctionName = jobState.FunctionName,
                IsForSpy = jobState.IsForSpy,
                FriendId = jobState.FriendId,
                JobId = jobState.JobId
            });
        }

        public List<string> DeleteJobState(JobStateViewModel jobState)
        {
            var jobIdList = new DeleteJobStateCommandHandler().Handle(new DeleteJobStateCommand
            {
                AccountId = jobState.AccountId,
                FunctionName = jobState.FunctionName,
                IsForSpy = jobState.IsForSpy,
                FriendId = jobState.FriendId
            });

            return jobIdList;
        }

        public void DeleteJobStateByJobId(string jobId)
        {
            new DeleteJobStateByJobIdCommandHandler().Handle(new DeleteJobStateByJobIdCommand
            {
                JobId = jobId
            });
        }

        public List<string> DeleteJobStateByAccount(long accountId, bool isSpy)
        {
            var jobIdList = new DeleteJobStateByAccounCommandHandler().Handle(new DeleteJobStateByAccounCommand
            {
                AccountId = accountId,
                IsForSpy = isSpy
            });

            return jobIdList;
        }
        public List<JobStateViewModel> GetStatesByAccountAndFunctionName(JobStateViewModel jobState)
        {
            var result = new GetStatesByModelCommandHandler().Handle(new GetStatesByModelCommand
            {
                AccountId = jobState.AccountId,
                FunctionName = jobState.FunctionName,
                IsForSpy = jobState.IsForSpy
            });

            return result.Select(model => new JobStateViewModel
            {
                AccountId = model.AccountId,
                JobId = model.JobId,
                FunctionName = model.FunctionName,
                IsForSpy = model.IsForSpy,
                FriendId = model.FriendId
            }).ToList();
        }
    }
}
