using Runner.Interfaces;
using Runner.Notices;
using Services.Services;
using Services.ViewModels.HomeModels;

namespace Runner.Runners.Community
{
    public class InvaitTheNewPageRunner : IRunner
    {
        public void Run(AccountViewModel account)
        {
            new GroupService(new NoticesProxy()).InviteToPage(account);
        }
    }
}
