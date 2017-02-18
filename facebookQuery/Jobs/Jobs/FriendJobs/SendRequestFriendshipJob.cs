using Constants.FunctionEnums;
using Hangfire;
using Services.ServiceTools;
using Services.ViewModels.HomeModels;

namespace Jobs.Jobs.FriendJobs
{
    public static class SendRequestFriendshipJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)] 
        public static void Run(AccountViewModel account)
        {
            if (!new FunctionPermissionManager().HasPermissionsByFacebookId(FunctionName.SendRequestFriendship, account.FacebookId))
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

            //jobStatusService.AddOrUpdateStatus(account.Id, JobNames.SendRequestFriendship.GetDiscription());

            new JobQueueService().AddToQueue(account.Id, FunctionName.SendRequestFriendship);

            //jobStatusService.AddOrUpdateStatus(account.Id, JobNames.SendRequestFriendship.GetDiscription());
        }
    }
}
