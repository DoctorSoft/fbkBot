using System;
using Constants.FunctionEnums;
using Hangfire;
using Jobs.JobsService;
using Runner.Notices;
using Services.Services;
using Services.ServiceTools;
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

            new JobStatusService().DeleteJobStatus(account.Id, FunctionName.SendRequestFriendship);

            var settings = new GroupService(new NoticesProxy()).GetSettings((long)account.GroupSettingsId);
            var sendRequestFriendshipsLaunchTime = new TimeSpan(settings.RetryTimeSendRequestFriendshipsHour, settings.RetryTimeSendRequestFriendshipsMin, settings.RetryTimeSendRequestFriendshipsSec);
            new BackgroundJobService().CreateBackgroundJob(account, FunctionName.SendRequestFriendship, sendRequestFriendshipsLaunchTime, true);
            
            new JobQueueService().AddToQueue(account.Id, FunctionName.SendRequestFriendship);
        }
    }
}
