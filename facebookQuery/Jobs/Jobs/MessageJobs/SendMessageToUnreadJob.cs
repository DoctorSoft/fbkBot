using System;
using Constants.FunctionEnums;
using Hangfire;
using Jobs.JobsService;
using Jobs.Models;
using Services.Models.BackgroundJobs;
using Services.Services;
using Services.ViewModels.JobStatusModels;
using Services.ViewModels.QueueViewModels;

namespace Jobs.Jobs.MessageJobs
{
    public static class SendMessageToUnreadJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public static void Run(RunJobModel runModel)
        {
            var account = runModel.Account;
            var forSpy = runModel.ForSpy;
            var friend = runModel.Friend;

            if (account.GroupSettingsId == null)
            {
                return;
            }

            var jobStatusModel = new JobStatusViewModel
            {
                AccountId = account.Id,
                FunctionName = FunctionName.SendMessageToUnread,
                IsForSpy = forSpy
            };

            new JobStatusService().DeleteJobStatus(jobStatusModel);

            var settings = new GroupService(new NoticeService()).GetSettings((long)account.GroupSettingsId);
            var sendUnreadLaunchTime = new TimeSpan(settings.RetryTimeSendUnreadHour, settings.RetryTimeSendUnreadMin, settings.RetryTimeSendUnreadSec);

            var model = new CreateBackgroundJobModel
            {
                Account = account,
                FunctionName = FunctionName.SendMessageToUnread,
                LaunchTime = sendUnreadLaunchTime,
                CheckPermissions = true,
                IsForSpy = forSpy
            };

            new BackgroundJobService().CreateBackgroundJob(model);

            var jobQueueModel = new JobQueueViewModel
            {
                AccountId = account.Id,
                FunctionName = FunctionName.SendMessageToUnread,
                IsForSpy = forSpy
            };

            new JobQueueService().AddToQueue(jobQueueModel);
        }
    }
}
