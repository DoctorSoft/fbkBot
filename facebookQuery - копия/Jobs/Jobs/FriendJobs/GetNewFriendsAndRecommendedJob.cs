using System;
using Constants.FunctionEnums;
using Hangfire;
using Jobs.JobsService;
using Services.Services;
using Services.ViewModels.HomeModels;

namespace Jobs.Jobs.FriendJobs
{
    public static class GetNewFriendsAndRecommendedJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public static void Run(AccountViewModel account)
        {
            if (account.GroupSettingsId == null)
            {
                return;
            }

            var settings = new GroupService().GetSettings((long)account.GroupSettingsId);
            var getNewAndRecommendedFriendsLaunchTime = new TimeSpan(settings.RetryTimeGetNewAndRecommendedFriendsHour, settings.RetryTimeGetNewAndRecommendedFriendsMin, settings.RetryTimeGetNewAndRecommendedFriendsSec);
            new BackgroundJobService().CreateBackgroundJob(account, FunctionName.GetNewFriendsAndRecommended, getNewAndRecommendedFriendsLaunchTime);

            new JobStatusService().AddOrUpdateJobStatus(FunctionName.GetNewFriendsAndRecommended, account.Id);

            new JobQueueService().AddToQueue(account.Id, FunctionName.GetNewFriendsAndRecommended);
        }
    }
}
