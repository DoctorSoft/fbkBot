using Constants.FunctionEnums;
using Hangfire;
using Services.Services;
using Services.ViewModels.FriendsModels;
using Services.ViewModels.HomeModels;

namespace Jobs.Jobs.DeleteFriendsJobs
{
    public static class IsWinkFriendsOfFriendsConditionJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public static void Run(AccountViewModel account, FriendViewModel friend)
        {
            if (account.GroupSettingsId == null)
            {
                return;
            }

            new JobStatusService().DeleteJobStatus(account.Id, FunctionName.IsWinkFriendsOfFriends, friend.Id);

            new JobQueueService().AddToQueue(account.Id, FunctionName.IsWinkFriendsOfFriends);
        }
    }
}
