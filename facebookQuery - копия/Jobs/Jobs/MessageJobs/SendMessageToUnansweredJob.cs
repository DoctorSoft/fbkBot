using System;
using Constants.FunctionEnums;
using Hangfire;
using Jobs.JobsService;
using Services.Services;
using Services.ViewModels.HomeModels;

namespace Jobs.Jobs.MessageJobs
{
    public static class SendMessageToUnansweredJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public static void Run(AccountViewModel account)
        {
            if (account.GroupSettingsId == null)
            {
                return;
            }

            var settings = new GroupService().GetSettings((long)account.GroupSettingsId);
            var sendUnansweredLaunchTime = new TimeSpan(settings.RetryTimeSendUnansweredHour, settings.RetryTimeSendUnansweredMin, settings.RetryTimeSendUnansweredSec);
            new BackgroundJobService().CreateBackgroundJob(account, FunctionName.SendMessageToUnanswered, sendUnansweredLaunchTime);

            new JobStatusService().AddOrUpdateJobStatus(FunctionName.SendMessageToUnanswered, account.Id);

            new JobQueueService().AddToQueue(account.Id, FunctionName.SendMessageToUnanswered);
        }
    }
}
