using System;
using Constants.FunctionEnums;
using Hangfire;
using Jobs.JobsService;
using Jobs.Models;
using Services.Models.BackgroundJobs;
using Services.Services;
using Services.ViewModels.JobStatusModels;
using Services.ViewModels.QueueViewModels;

namespace Jobs.Jobs.WinksJobs
{
    public static class WinkFriendsJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public static void Run(RunJobModel runModel)
        {
            var account = runModel.Account;
            var forSpy = runModel.ForSpy;

            if (account.GroupSettingsId == null)
            {
                return;
            }

            var jobStatusModel = new JobStatusViewModel
            {
                AccountId = account.Id,
                FunctionName = FunctionName.Wink,
                IsForSpy = forSpy
            };

            new JobStatusService().DeleteJobStatus(jobStatusModel);

            var settings = new GroupService(new NoticeService()).GetSettings((long) account.GroupSettingsId);
            var winkFriendsLaunchTime = new TimeSpan(settings.RetryTimeForWinkFriendsHour, settings.RetryTimeForWinkFriendsMin, settings.RetryTimeForWinkFriendsSec);
            
            var model = new CreateBackgroundJobModel
            {
                Account = account,
                FunctionName = FunctionName.Wink,
                LaunchTime = winkFriendsLaunchTime,
                CheckPermissions = true,
                IsForSpy = forSpy
            };

            new BackgroundJobService().CreateBackgroundJob(model);

            var jobQueueModel = new JobQueueViewModel
            {
                AccountId = account.Id,
                FunctionName = FunctionName.Wink,
                IsForSpy = forSpy
            };

            new JobQueueService().AddToQueue(jobQueueModel);
        }
    }
}
