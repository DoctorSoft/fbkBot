using Hangfire;
using Services.Services;

namespace Jobs.Jobs.FriendJobs
{
    public static class RefreshFriendsJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public static void Run(long userId)
        {
            new HomeService().GetFriends(userId);
        }
    }
}
