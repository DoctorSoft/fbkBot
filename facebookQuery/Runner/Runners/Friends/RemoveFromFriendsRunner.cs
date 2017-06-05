
using Runner.Interfaces;
using Services.Services;

namespace Runner.Runners.Friends
{
    public class RemoveFromFriendsRunner : IRunner
    {
        public void Run(IRunnerModel model)
        {
            var account = model.Account;
            var friend = model.Friend;

            new FriendsService(new NoticeService()).RemoveFriend(account.Id, friend.Id);
        }
    }
}
