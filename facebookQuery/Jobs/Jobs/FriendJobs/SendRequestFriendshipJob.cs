using Constants.FunctionEnums;
using Hangfire;
using Services.Services;
using Services.ServiceTools;

namespace Jobs.Jobs.FriendJobs
{
    public static class SendRequestFriendshipJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail), Queue("sendrequestfriendship", Order = 1)] 
        public static void Run(long userId)
        {
            if (!new FunctionPermissionManager().HasPermissionsByFacebookId(FunctionName.SendRequestFriendship, userId))
            {
                return;
            }

            new FriendsService().SendRequestFriendship(userId);
        }
    }
}
