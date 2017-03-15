using System;
using Constants.FunctionEnums;
using Hangfire;
using Jobs.JobsService;
using Runner.Notices;
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

            new JobStatusService().DeleteJobStatus(account.Id, FunctionName.SendMessageToUnread);

            var settings = new GroupService(new NoticesProxy()).GetSettings((long)account.GroupSettingsId);
            var sendUnreadLaunchTime = new TimeSpan(settings.RetryTimeSendUnreadHour, settings.RetryTimeSendUnreadMin, settings.RetryTimeSendUnreadSec);
            new BackgroundJobService().CreateBackgroundJob(account, FunctionName.SendMessageToUnread, sendUnreadLaunchTime, true);
            
            new JobQueueService().AddToQueue(account.Id, FunctionName.SendMessageToUnread);
        }
    }
}
