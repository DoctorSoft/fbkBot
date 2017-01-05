using Hangfire;
using Jobs.JobsService;
using Services.Services;
using Services.ViewModels.HomeModels;

namespace Jobs.Jobs.SpyJobs
{
    public static class AnalyzeFriendsJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail), Queue("analyzefriends", Order = 1)]
        public static void Run(AccountViewModel account)
        {
//            if (!new FunctionPermissionManager().HasPermissionsByFacebookId(FunctionName.AnalyzeFriends, account.FacebookId))
//            {
//                return;
//            }

            new SpyService(new JobService()).AnalyzeFriends(account);
        }
    }
}
