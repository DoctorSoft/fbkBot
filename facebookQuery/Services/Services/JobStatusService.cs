using System;
using Constants.FunctionEnums;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.JobStatus;
using DataBase.QueriesAndCommands.Queries.JobStatus;
using Services.ViewModels.JobStatusModels;

namespace Services.Services
{
    public class JobStatusService
    {
        public JobStatusModel GetJobStatus(FunctionName functionName)
        {
            var jobStatus = new GetJobStatusQueryHandler(new DataBaseContext()).Handle(new GetJobStatusQuery            
            {
                FunctionName = functionName
            });

            return new JobStatusModel
            {
                LastLaunchDateTime = jobStatus.LastLaunchDateTime,
                Id = jobStatus.Id,
                FunctionName = jobStatus.FunctionName
            };
        }

        public void AddOrUpdateJobStatus(FunctionName functionName)
        {
            new AddOrUpdateJobStatusCommandHandler(new DataBaseContext()).Handle(new AddOrUpdateJobStatusCommand
            {
                FunctionName = functionName,
                LaunchDateTime = DateTime.Now
            });
        }

        public void DeleteJobStatus(FunctionName functionName)
        {
            new DeleteJobStatusCommandHandler(new DataBaseContext()).Handle(new DeleteJobStatusCommand
            {
                FunctionName = functionName
            });
        }
    }
}
