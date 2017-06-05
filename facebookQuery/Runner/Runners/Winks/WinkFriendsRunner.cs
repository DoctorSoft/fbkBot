using Runner.Interfaces;
using Services.Services;

namespace Runner.Runners.Winks
{
    public class WinkFriendsRunner : IRunner
    {
        public void Run(IRunnerModel model)
        {
            var account = model.Account;

            new WinksService(new NoticeService()).WinkFriend(account);
        }
    }
}
