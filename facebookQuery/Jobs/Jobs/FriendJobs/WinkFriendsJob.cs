using System;
using Constants.FunctionEnums;
using Hangfire;
using Jobs.JobsService;
using Jobs.Notices;
using Services.Models.BackgroundJobs;
using Services.Services;
using Services.ViewModels.FriendsModels;
using Services.ViewModels.HomeModels;

namespace Jobs.Jobs.FriendJobs
{
    public static class WinkFriendsJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public static void Run(AccountViewModel account)
        {
            if (account.GroupSettingsId == null)
            {
                return;
            }

            new JobStatusService().DeleteJobStatus(account.Id, FunctionName.Wink, null);

            var settings = new GroupService(new NoticesProxy()).GetSettings((long) account.GroupSettingsId);
            var winkFriendsLaunchTime = new TimeSpan(settings.RetryTimeForWinkFriendsHour, settings.RetryTimeForWinkFriendsMin, settings.RetryTimeForWinkFriendsSec);
            
            var friend = new FriendsService(new NoticesProxy()).GetFriendToWink(account);
            long friendId = 0;
            if (friend != null)
            {
                friendId = friend.Id;
            }

            var model = new CreateBackgroundJobModel
            {
                Account = account,
                FunctionName = FunctionName.Wink,
                LaunchTime = winkFriendsLaunchTime,
                CheckPermissions = true
            };

            new BackgroundJobService().CreateBackgroundJob(model);

            new JobQueueService().AddToQueue(account.Id, FunctionName.Wink, friendId);
        }
    }
}
