using Constants.FunctionEnums;
using Hangfire;
using Services.Services;
using Services.ServiceTools;

namespace Jobs.Jobs.FriendJobs
{
    public static class GetNewFriendsAndRecommendedJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public static void Run(long userId)
        {
            if (!new FunctionPermissionManager().HasPermissionsByFacebookId(FunctionName.GetNewFriendsAndRecommended, userId))
            {
                return;
            }

            new FriendsService().GetNewFriendsAndRecommended(userId);
        }
    }
}
