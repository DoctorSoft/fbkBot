using Constants.FunctionEnums;
using Hangfire;
using Services.Services;
using Services.ServiceTools;
using Services.ViewModels.HomeModels;

namespace Jobs.Jobs.CommunityJobs
{
    public static class JoinTheNewGroupsAndPagesJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public static void Run(AccountViewModel account)
        {
            if (account.GroupSettingsId == null)
            {
                return;
            }
            
            new JobStatusService().DeleteJobStatus(account.Id, FunctionName.JoinTheNewGroupsAndPages, null);

            new JobQueueService().AddToQueue(account.Id, FunctionName.JoinTheNewGroupsAndPages);
        }
    }
}
