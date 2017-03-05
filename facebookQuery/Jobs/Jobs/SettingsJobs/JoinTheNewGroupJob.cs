using Constants.FunctionEnums;
using Hangfire;
using Services.Services;
using Services.ViewModels.HomeModels;

namespace Jobs.Jobs.SettingsJobs
{
    public static class JoinTheNewGroupJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public static void Run(AccountViewModel account)
        {
            if (account.GroupSettingsId == null)
            {
                return;
            }
            
            new JobStatusService().AddOrUpdateJobStatus(FunctionName.JoinTheNewGroup, account.Id);

            new JobQueueService().AddToQueue(account.Id, FunctionName.JoinTheNewGroup);
        }
    }
}
