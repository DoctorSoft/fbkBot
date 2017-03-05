using System;
using Constants.FunctionEnums;
using Hangfire;
using Jobs.JobsService;
using Services.Services;
using Services.ViewModels.HomeModels;

namespace Jobs.Jobs.MessageJobs
{
    public static class SendMessageToNewFriendsJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public static void Run(AccountViewModel account)
        {
            if (account.GroupSettingsId == null)
            {
                return;
            }

            var settings = new GroupService().GetSettings((long)account.GroupSettingsId);
            var sendNewFriendLaunchTime = new TimeSpan(settings.RetryTimeSendNewFriendHour, settings.RetryTimeSendNewFriendMin, settings.RetryTimeSendNewFriendSec);
            new BackgroundJobService().CreateBackgroundJob(account, FunctionName.SendMessageToNewFriends, sendNewFriendLaunchTime);

            new JobStatusService().AddOrUpdateJobStatus(FunctionName.SendMessageToNewFriends, account.Id);

            new JobQueueService().AddToQueue(account.Id, FunctionName.SendMessageToNewFriends);
        }
    }
}
