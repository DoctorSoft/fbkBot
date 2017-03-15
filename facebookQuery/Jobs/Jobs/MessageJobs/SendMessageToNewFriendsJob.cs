using System;
using Constants.FunctionEnums;
using Hangfire;
using Jobs.JobsService;
using Runner.Notices;
using Services.Services;
using Services.ServiceTools;
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

            new JobStatusService().DeleteJobStatus(account.Id, FunctionName.SendMessageToNewFriends);

            var settings = new GroupService(new NoticesProxy()).GetSettings((long)account.GroupSettingsId);
            var sendNewFriendLaunchTime = new TimeSpan(settings.RetryTimeSendNewFriendHour, settings.RetryTimeSendNewFriendMin, settings.RetryTimeSendNewFriendSec);
            new BackgroundJobService().CreateBackgroundJob(account, FunctionName.SendMessageToNewFriends, sendNewFriendLaunchTime, true);
            
            new JobQueueService().AddToQueue(account.Id, FunctionName.SendMessageToNewFriends);
        }
    }
}
