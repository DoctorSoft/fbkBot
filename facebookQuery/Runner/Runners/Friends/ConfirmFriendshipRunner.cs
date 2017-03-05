using Runner.Interfaces;
using Services.Services;
using Services.ViewModels.HomeModels;
using NoticesProxy = Runner.Notices.NoticesProxy;

namespace Runner.Runners.Friends
{
    public class ConfirmFriendshipRunner : IRunner
    {
        public void Run(AccountViewModel account)
        {
            new FriendsService(new NoticesProxy()).ConfirmFriendship(account);
        }
    }
}
