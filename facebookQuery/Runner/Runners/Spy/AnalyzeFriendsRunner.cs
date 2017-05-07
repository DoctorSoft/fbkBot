using Jobs.JobsService;
using Runner.Interfaces;
using Services.Services;

namespace Runner.Runners.Spy
{
    public class AnalyzeFriendsRunner : IRunner
    {
        public void Run(IRunnerModel model)
        {
            var account = model.Account;

            new SpyService(new JobService()).AnalyzeFriends(account);
        }
    }
}
