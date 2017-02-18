using Constants.FunctionEnums;
using Hangfire;
using Services.ServiceTools;
using Services.ViewModels.HomeModels;

namespace Jobs.Jobs.MessageJobs
{
    public static class SendMessageToUnreadJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public static void Run(AccountViewModel account)
        {
            if (!new FunctionPermissionManager().HasPermissionsByFacebookId(FunctionName.SendMessageToUnread, account.FacebookId))
            {
                return;
            }
            
            if (!new AccountManager().HasAWorkingProxy(account.Id))
            {
                return;
            }

            if (!new AccountManager().HasAWorkingAuthorizationData(account.Id))
            {
                return;
            }

            //var jobStatusService = new JobStatusService();

            //jobStatusService.AddOrUpdateStatus(account.Id, JobNames.SendMessageToUnread.GetDiscription());

            new JobQueueService().AddToQueue(account.Id, FunctionName.SendMessageToUnread);

            //jobStatusService.AddOrUpdateStatus(account.Id, JobNames.SendMessageToUnread.GetDiscription());
        }
    }
}
