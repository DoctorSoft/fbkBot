using Hangfire;
using Jobs.Notices;
using Services.Services;
using Services.ViewModels.HomeModels;

namespace Jobs.Jobs.FriendJobs
{
    public static class CheckFriendsAtTheEndTimeConditionsJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public static void Run(AccountViewModel account)
        {
            if (account.GroupSettingsId == null)
            {
                return;
            }

            new FriendsService(new NoticesProxy()).CheckFriendsAtTheEndTimeConditions(account);
        }
    }
}
