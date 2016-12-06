using Hangfire;
using Services.Services;

namespace Jobs.Jobs.MessageJobs
{
    public static class GetUnreadMessagesJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public static void Run(long userId)
        {
            //new FacebookMessagesService().GetUnreadMessages(userId);
        }
    }
}
