using System;
using Constants.FunctionEnums;
using Hangfire;
using Jobs.JobsService;
using Runner.Notices;
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
            new JobStatusService().DeleteJobStatus(account.Id, FunctionName.RefreshFriends);

            var settings = new GroupService(new NoticesProxy()).GetSettings((long)account.GroupSettingsId);
            var refreshFriendsLaunchTime = new TimeSpan(settings.RetryTimeRefreshFriendsHour, settings.RetryTimeRefreshFriendsMin, settings.RetryTimeRefreshFriendsSec);
            new BackgroundJobService().CreateBackgroundJob(account, FunctionName.RefreshFriends, refreshFriendsLaunchTime, true);
            
            new JobQueueService().AddToQueue(account.Id, FunctionName.RefreshFriends);
        }
    }
}
