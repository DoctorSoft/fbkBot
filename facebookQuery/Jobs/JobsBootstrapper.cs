using System;
using Hangfire;

namespace Jobs
{
    public static class JobsBootstrapper
    {
        public static void Run()
        {
            
        }

        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public static void FailedJob()
        {
            try
            {
                throw new Exception("Fail job message");
            }
            catch (Exception exception)
            {
                RecurringJob.RemoveIfExists("Failed Job");
                throw exception;
            }
        }

        public static void SetUpJobs()
        {
            RecurringJob.AddOrUpdate("Test Job", () => Run(), Cron.Hourly);
            RecurringJob.AddOrUpdate("Failed Job", () => FailedJob(), Cron.Daily);
        }
    }
}
