using Jobs.Notices;
using Runner.Interfaces;
using Services.Services;

namespace Runner.Runners.Community
{
    public class JoinTheNewGroupsAndPagesRunner : IRunner
    {
        public void Run(IRunnerModel model)
        {
            var account = model.Account;
            new GroupService(new NoticesProxy()).JoinTheNewGroupsAndPages(account);
        }
    }
}
