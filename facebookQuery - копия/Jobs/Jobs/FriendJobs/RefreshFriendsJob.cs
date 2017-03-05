using System;
using Constants.FunctionEnums;
using Hangfire;
using Jobs.JobsService;
using Services.Services;
using Services.ViewModels.HomeModels;

namespace Jobs.Jobs.FriendJobs
{
    public static class RefreshFriendsJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public static void Run(AccountViewModel account)
        {
            if (account.GroupSettingsId == null)
            {
                return;
            }

            var settings = new GroupService().GetSettings((long)account.GroupSettingsId);
            var refreshFriendsLaunchTime = new TimeSpan(settings.RetryTimeRefreshFriendsHour, settings.RetryTimeRefreshFriendsMin, settings.RetryTimeRefreshFriendsSec);
            new BackgroundJobService().CreateBackgroundJob(account, FunctionName.RefreshFriends, refreshFriendsLaunchTime);

            new JobStatusService().AddOrUpdateJobStatus(FunctionName.RefreshFriends, account.Id);

            new JobQueueService().AddToQueue(account.Id, FunctionName.RefreshFriends);
        }
    }
}
