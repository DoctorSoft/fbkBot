using Hangfire;
using Services.Services;

namespace Jobs.Jobs.Notices
{
    public static class ClearNoticesJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public static void Run()
        {
            new NoticeService().ClearOldNotice();
        }
    }
}
