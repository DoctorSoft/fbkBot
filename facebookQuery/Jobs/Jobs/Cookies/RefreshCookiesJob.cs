using System;
using Constants.FunctionEnums;
using Jobs.JobsService;
using Jobs.Models;
using Services.Models.BackgroundJobs;
using Services.Services;
using Services.ViewModels.JobStatusModels;
using Services.ViewModels.QueueViewModels;

namespace Jobs.Jobs.Cookies
{
    public static class RefreshCookiesJob
    {
        public static void Run(RunJobModel runModel)
        {
            var account = runModel.Account;
            var isForSpy = runModel.ForSpy;
            var friend = runModel.Friend;

            var jobStatusModel = new JobStatusViewModel
            {
                AccountId = account.Id,
                FunctionName = FunctionName.RefreshCookies,
                IsForSpy = isForSpy
            };

            new JobStatusService().DeleteJobStatus(jobStatusModel);

            var model = new CreateBackgroundJobModel
            {
                Account = account,
                FunctionName = FunctionName.RefreshCookies,
                LaunchTime = new TimeSpan(2, 0, 0),
                CheckPermissions = false,
                IsForSpy = isForSpy
            };

            new BackgroundJobService().CreateBackgroundJob(model);

            var jobQueueModel = new JobQueueViewModel
            {
                AccountId = account.Id,
                FunctionName = FunctionName.RefreshCookies,
                IsForSpy = isForSpy
            };

            new JobQueueService().AddToQueue(jobQueueModel);
        }
    }
}
