using System;
using Constants.FunctionEnums;
using Hangfire;
using Jobs.JobsService;
using Services.Services;
using Services.ViewModels.HomeModels;

namespace Jobs.Jobs.FriendJobs
{
    public static class SendRequestFriendshipJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)] 
        public static void Run(AccountViewModel account)
        {
            if (account.GroupSettingsId == null)
            {
                return;
            }

            var settings = new GroupService().GetSettings((long)account.GroupSettingsId);
            var sendRequestFriendshipsLaunchTime = new TimeSpan(settings.RetryTimeSendRequestFriendshipsHour, settings.RetryTimeSendRequestFriendshipsMin, settings.RetryTimeSendRequestFriendshipsSec);
            new BackgroundJobService().CreateBackgroundJob(account, FunctionName.SendRequestFriendship, sendRequestFriendshipsLaunchTime, true);

            new JobStatusService().AddOrUpdateJobStatus(FunctionName.SendRequestFriendship, account.Id);

            new JobQueueService().AddToQueue(account.Id, FunctionName.SendRequestFriendship);
        }
    }
}
