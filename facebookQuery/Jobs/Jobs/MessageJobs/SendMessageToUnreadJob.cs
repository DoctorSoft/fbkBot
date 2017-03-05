using System;
using Constants.FunctionEnums;
using Hangfire;
using Jobs.JobsService;
using Services.Services;
using Services.ViewModels.HomeModels;

namespace Jobs.Jobs.MessageJobs
{
    public static class SendMessageToUnreadJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public static void Run(AccountViewModel account)
        {
            if (account.GroupSettingsId == null)
            {
                return;
            }

            var settings = new GroupService().GetSettings((long)account.GroupSettingsId);
            var sendUnreadLaunchTime = new TimeSpan(settings.RetryTimeSendUnreadHour, settings.RetryTimeSendUnreadMin, settings.RetryTimeSendUnreadSec);
            new BackgroundJobService().CreateBackgroundJob(account, FunctionName.SendMessageToUnread, sendUnreadLaunchTime, true);

            new JobStatusService().AddOrUpdateJobStatus(FunctionName.SendMessageToUnread, account.Id);

            new JobQueueService().AddToQueue(account.Id, FunctionName.SendMessageToUnread);
        }
    }
}
