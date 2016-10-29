using Hangfire;
using Services.Services;

namespace Jobs.Jobs.MessageJobs
{
    public static class SendMessageJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public static void Run(long userId, long receiverId)
        {
            new HomeService().SendMessage(userId, receiverId);
        }
    }
}
