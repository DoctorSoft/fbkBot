using Constants.FunctionEnums;
using Hangfire;
using Services.Services;
using Services.ServiceTools;

namespace Jobs.Jobs.FriendJobs
{
    public static class RefreshFriendsJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public static void Run(long userId)
        {
            if (!new FunctionPermissionManager().HasPermissionsByFacebookId(FunctionName.RefreshFriends, userId))
            {
                return;
            }

            new FriendsService().GetFriendsOfFacebook(userId);
        }
    }
}
