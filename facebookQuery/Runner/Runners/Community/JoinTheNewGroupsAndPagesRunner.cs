using Jobs.Notices;
using Runner.Interfaces;
using Services.Services;
using Services.ViewModels.HomeModels;

namespace Runner.Runners.Community
{
    public class JoinTheNewGroupsAndPagesRunner : IRunner
    {
        public void Run(AccountViewModel account)
        {
            new GroupService(new NoticesProxy()).JoinTheNewGroupsAndPages(account);
        }
    }
}
