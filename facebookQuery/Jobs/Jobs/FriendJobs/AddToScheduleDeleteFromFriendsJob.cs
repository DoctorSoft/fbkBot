using Constants.FunctionEnums;
using Hangfire;
using Jobs.Notices;
using Services.Services;
using Services.ViewModels.HomeModels;

namespace Jobs.Jobs.FriendJobs
{
    public static class AddToScheduleDeleteFromFriendsJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public static void Run(AccountViewModel account)
        {
            new FriendsService(new NoticesProxy()).GetFriendsToQueueDeletion(account);
        }
    }
}
