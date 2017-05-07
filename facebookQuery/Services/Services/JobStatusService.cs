using System.Collections.Generic;
using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.JobStatus;
using DataBase.QueriesAndCommands.Commands.JobStatus.DeleteJobStatusesByAccountId;
using DataBase.QueriesAndCommands.Queries.JobStatus;
using Services.ViewModels.JobStatusModels;

namespace Services.Services
{
    public class JobStatusService
    {
        public bool JobIsInRun(JobStatusViewModel model)
        {
            var jobStatus = new CheckJobStatusQueryHandler(new DataBaseContext()).Handle(new CheckJobStatusQuery            
            {
                AccountId = model.AccountId,
                FunctionName = model.FunctionName,
                IsForSpy = model.IsForSpy,
                FriendId = model.FriendId
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
                FriendId = model.FriendId,
                IsForSpy = model.IsForSpy
            });
        }

        public List<JobStatusViewModel> GetJobStatus(JobStatusViewModel model)
        {
            var jobStatusList = new GetJobStatusQueryHandler(new DataBaseContext()).Handle(new GetJobStatusQuery
            {
                AccountId = model.AccountId,
                FunctionName = model.FunctionName,
                FriendId = model.FriendId,
                IsForSpy = model.IsForSpy
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
                FriendId = jobStatus.FriendId,
                IsForSpy = jobStatus.IsForSpy
            }).ToList();
        }

        public void DeleteJobStatus(JobStatusViewModel model)
        {
            new DeleteJobStatusCommandHandler(new DataBaseContext()).Handle(new DeleteJobStatusCommand
            {
                AccountId = model.AccountId,
                FunctionName = model.FunctionName,
                FriendId = model.FriendId,
                IsForSpy = model.IsForSpy
            });
        }

        public List<string> DeleteJobStatusesByAccountId(JobStatusViewModel model)
        {
            var jobsId = new DeleteJobStatusesByAccountIdCommandHandler(new DataBaseContext()).Handle(new DeleteJobStatusesByAccountIdCommand
            {
                AccountId = model.AccountId,
                IsForSpy = model.IsForSpy,
                FunctionName = model.FunctionName
            });

            return jobsId;
        }
    }
}
