using Constants.FunctionEnums;
using Hangfire;
using Services.Services;
using Services.ViewModels.FriendsModels;
using Services.ViewModels.HomeModels;

namespace Jobs.Jobs.DeleteFriendsJobs
{
    public static class RemoveFromFriendsJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public static void Run(AccountViewModel account, FriendViewModel friend)
        {
            if (account.GroupSettingsId == null)
            {
                return;
            }

            if (friend == null)
            {
                return;
            }

            new JobStatusService().DeleteJobStatus(account.Id, FunctionName.RemoveFromFriends, friend.Id);

            new JobQueueService().AddToQueue(account.Id, FunctionName.RemoveFromFriends, friend.Id);
        }
    }
}
