using Constants.FunctionEnums;
using Hangfire;
using Services.Services;
using Services.ServiceTools;

namespace Jobs.Jobs.FriendJobs
{
    public static class ConfirmFriendshipJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail), Queue("confirmfriendship", Order = 1)] 
        public static void Run(long userId)
        {
            if (!new FunctionPermissionManager().HasPermissionsByFacebookId(FunctionName.ConfirmFriendship, userId))
            {
                return;
            }

            new FriendsService().ConfirmFriendship(userId);
        }
    }
}
