using Jobs.Notices;
using Runner.Interfaces;
using Services.Services;

namespace Runner.Runners.Friends
{
    public class WinkFriendsRunner : IRunner
    {
        public void Run(IRunnerModel model)
        {
            var account = model.Account;
            var friend = model.Friend;

            new FriendsService(new NoticesProxy()).WinkFriend(account, friend.Id);
        }
    }
}
