using System.Collections.Generic;
using System.Linq;
using Constants.FunctionEnums;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.JobStatus;
using DataBase.QueriesAndCommands.Commands.JobStatus.DeleteJobStatusesByAccountId;
using DataBase.QueriesAndCommands.Queries.JobStatus;
using Services.ViewModels.JobStatusModels;

namespace Services.Services
{
    public class JobStatusService
    {
        public bool JobIsInRun(long accountId, FunctionName functionName)
        {
            var jobStatus = new CheckJobStatusQueryHandler(new DataBaseContext()).Handle(new CheckJobStatusQuery            
            {
                AccountId = accountId,
                FunctionName = functionName
            });

            return jobStatus;
        }

        public void AddJobStatus(JobStatusViewModel model)
        {
            new AddJobStatusCommandHandler(new DataBaseContext()).Handle(new AddJobStatusCommand
            {
                AccountId = model.AccountId,
                FunctionName = model.FunctionName,
                LaunchTime = model.LaunchTime,
                JobId = model.JobId,
                FriendId = model.FriendId
            });
        }

        public List<JobStatusViewModel> GetJobStatus(long accountId, FunctionName functionName, long? friendId)
        {
            var jobStatusList = new GetJobStatusQueryHandler(new DataBaseContext()).Handle(new GetJobStatusQuery
            {
                AccountId = accountId,
                FunctionName = functionName,
                FriendId = friendId
            });

            if (jobStatusList == null || jobStatusList.Count == 0)
            {
                return null;
            }

            return jobStatusList.Select(jobStatus => new JobStatusViewModel
            {
                AccountId = jobStatus.AccountId,
                Id = jobStatus.Id,
                FunctionName = jobStatus.FunctionName,
                AddDateTime = jobStatus.AddDateTime,
                JobId = jobStatus.JobId,
                LaunchTime = jobStatus.LaunchTime,
                FriendId = jobStatus.FriendId
            }).ToList();
        }
        public void DeleteJobStatus(long accountId, FunctionName functionName, long? friendId)
        {
            new DeleteJobStatusCommandHandler(new DataBaseContext()).Handle(new DeleteJobStatusCommand
            {
                AccountId = accountId,
                FunctionName = functionName,
                FriendId = friendId
            });
        }

        public List<string> DeleteJobStatusesByAccountId(long accountId)
        {
            var jobsId = new DeleteJobStatusesByAccountIdCommandHandler(new DataBaseContext()).Handle(new DeleteJobStatusesByAccountIdCommand
            {
                AccountId = accountId
            });

            return jobsId;
        }
    }
}
