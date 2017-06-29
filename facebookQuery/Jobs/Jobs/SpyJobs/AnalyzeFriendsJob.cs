using Constants.FunctionEnums;
using Hangfire;
using Services.Services;
using Services.ServiceTools;
using Services.ViewModels.HomeModels;
using Services.ViewModels.QueueViewModels;

namespace Jobs.Jobs.SpyJobs
{
    public static class AnalyzeFriendsJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]

        public static void Run(AccountViewModel account)
        {
            if (!new FunctionPermissionManager().HasPermissionsForSpy(FunctionName.AnalyzeFriends, account.FacebookId))
            {
                return;
            }

            var jobQueue = new JobQueueViewModel
            {
                AccountId = account.Id,
                FunctionName = FunctionName.AnalyzeFriends,
                IsForSpy = true
            };

            new JobQueueService().AddToQueue(jobQueue);
        }
    }
}
