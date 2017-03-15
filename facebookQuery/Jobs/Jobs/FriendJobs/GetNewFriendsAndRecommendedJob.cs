using System;
using Constants.FunctionEnums;
using Hangfire;
using Jobs.JobsService;
using Runner.Notices;
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

            new JobStatusService().DeleteJobStatus(account.Id, FunctionName.GetNewFriendsAndRecommended);

            var settings = new GroupService(new NoticesProxy()).GetSettings((long)account.GroupSettingsId);
            var getNewAndRecommendedFriendsLaunchTime = new TimeSpan(settings.RetryTimeGetNewAndRecommendedFriendsHour, settings.RetryTimeGetNewAndRecommendedFriendsMin, settings.RetryTimeGetNewAndRecommendedFriendsSec);
            new BackgroundJobService().CreateBackgroundJob(account, FunctionName.GetNewFriendsAndRecommended, getNewAndRecommendedFriendsLaunchTime, true);
            
            new JobQueueService().AddToQueue(account.Id, FunctionName.GetNewFriendsAndRecommended);
        }
    }
}
