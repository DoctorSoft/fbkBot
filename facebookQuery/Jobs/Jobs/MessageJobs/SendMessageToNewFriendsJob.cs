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
    public static class SendMessageToNewFriendsJob
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
                FunctionName = FunctionName.SendMessageToNewFriends,
                IsForSpy = forSpy
            };

            new JobStatusService().DeleteJobStatus(jobStatusModel);

            var settings = new GroupService(new NoticeService()).GetSettings((long)account.GroupSettingsId);
            var sendNewFriendLaunchTime = new TimeSpan(settings.RetryTimeSendNewFriendHour, settings.RetryTimeSendNewFriendMin, settings.RetryTimeSendNewFriendSec);

            var model = new CreateBackgroundJobModel
            {
                Account = account,
                FunctionName = FunctionName.SendMessageToNewFriends,
                LaunchTime = sendNewFriendLaunchTime,
                CheckPermissions = true,
                IsForSpy = forSpy
            };

            new BackgroundJobService().CreateBackgroundJob(model);

            var jobQueueModel = new JobQueueViewModel
            {
                AccountId = account.Id,
                FunctionName = FunctionName.SendMessageToNewFriends,
                IsForSpy = forSpy
            };

            new JobQueueService().AddToQueue(jobQueueModel);
        }
    }
}
