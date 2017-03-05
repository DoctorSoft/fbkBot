using System;
using Constants.FunctionEnums;
using Hangfire;
using Jobs.JobsService;
using Services.Services;
using Services.ViewModels.HomeModels;

namespace Jobs.Jobs.FriendJobs
{
    public static class ConfirmFriendshipJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public static void Run(AccountViewModel account)
        {
            if (account.GroupSettingsId == null)
            {
                return;
            }

            var settings = new GroupService().GetSettings((long) account.GroupSettingsId);
            var confirmFriendshipsLaunchTime = new TimeSpan(settings.RetryTimeConfirmFriendshipsHour, settings.RetryTimeConfirmFriendshipsMin, settings.RetryTimeConfirmFriendshipsSec);
            new BackgroundJobService().CreateBackgroundJob(account, FunctionName.ConfirmFriendship, confirmFriendshipsLaunchTime);

            new JobStatusService().AddOrUpdateJobStatus(FunctionName.ConfirmFriendship, account.Id);

            new JobQueueService().AddToQueue(account.Id, FunctionName.ConfirmFriendship);
        }
    }
}
