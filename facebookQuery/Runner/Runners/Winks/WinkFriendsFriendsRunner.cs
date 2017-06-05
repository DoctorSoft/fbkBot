
using Runner.Interfaces;
using Services.Services;

namespace Runner.Runners.Winks
{
    public class WinkFriendsFriendsRunner : IRunner
    {
        public void Run(IRunnerModel model)
        {
            var account = model.Account;

            new WinksService(new NoticeService()).WinkFriendsFriends(account);
        }
    }
}
