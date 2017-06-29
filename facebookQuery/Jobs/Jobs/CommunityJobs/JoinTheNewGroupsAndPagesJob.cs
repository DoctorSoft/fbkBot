using Hangfire;
using Jobs.Interfaces;

namespace Jobs.Jobs.CommunityJobs
{
    public class JoinTheNewGroupsAndPagesJob : IRunJob
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
        }
    }
}
