
using Runner.Interfaces;
using Services.Services;

namespace Runner.Runners.Community
{
    public class InvaitTheNewPageRunner : IRunner
    {
        public void Run(IRunnerModel model)
        {
            var account = model.Account;
             new GroupService(new NoticeService()).InviteToPage(account);
        }
    }
}
