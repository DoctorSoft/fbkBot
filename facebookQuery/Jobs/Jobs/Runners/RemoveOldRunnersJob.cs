using Hangfire;
using Services.Services.Runners;

namespace Jobs.Jobs.Runners
{
    public static class RemoveOldRunnersJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public static void Run()
        {
            new RunnerService().DeleteOldRunners();
        }
    }
}
