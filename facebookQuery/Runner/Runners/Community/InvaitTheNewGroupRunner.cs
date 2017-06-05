
using Runner.Interfaces;
using Services.Services;

namespace Runner.Runners.Community
{
    public class InvaitTheNewGroupRunner : IRunner
    {
        public void Run(IRunnerModel model)
        {
            var account = model.Account;

            new GroupService(new NoticeService()).InviteToGroup(account);
        }
    }
}
