using Hangfire;
using Jobs.Interfaces;

namespace Jobs.Jobs.DeleteFriendsJobs
{
    public class RemoveFromFriendsJob : IRunJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public void Run(IRunJobModel runModel)
        {
            var account = runModel.Account;
            var forSpy = runModel.ForSpy;
            var friend = runModel.Friend;

            if (account.GroupSettingsId == null)
            {
                return;
            }

            if (friend == null)
            {
                return;
            }
        }
    }
}
