using System;
using Constants.FunctionEnums;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.JobStatus;
using DataBase.QueriesAndCommands.Queries.JobStatus;
using Hangfire.Common;
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

        public void AddJobStatus(long accountId, FunctionName functionName, TimeSpan launchTime, string jobId)
        {
            new AddJobStatusCommandHandler(new DataBaseContext()).Handle(new AddJobStatusCommand
            {
                AccountId = accountId,
                FunctionName = functionName,
                LaunchDateTime = launchTime,
                JobId = jobId
            });
        }

        public JobStatusViewModel GetJobStatus(long accountId, FunctionName functionName)
        {
            var jobStatus = new GetJobStatusQueryHandler(new DataBaseContext()).Handle(new GetJobStatusQuery
            {
                AccountId = accountId,
                FunctionName = functionName
            });

            return new JobStatusViewModel
            {
                AccountId = jobStatus.AccountId,
                Id = jobStatus.Id,
                FunctionName = jobStatus.FunctionName,
                AddDateTime = jobStatus.AddDateTime,
                JobId = jobStatus.JobId,
                LaunchDateTime = jobStatus.LaunchDateTime
            };
        }
        public void DeleteJobStatus(long accountId, FunctionName functionName)
        {
            new DeleteJobStatusCommandHandler(new DataBaseContext()).Handle(new DeleteJobStatusCommand
            {
                AccountId = accountId,
                FunctionName = functionName
            });
        }
    }
}
