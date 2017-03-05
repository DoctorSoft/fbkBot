using Hangfire;

namespace Jobs.Jobs.MessageJobs
{
    public static class GetUnreadMessagesJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail), /*Queue("RefreshFriends")*/] //Add queue when you need
        public static void Run(long userId)
        {
            //new FacebookMessagesService().GetUnreadMessages(userId);
        }
    }
}
