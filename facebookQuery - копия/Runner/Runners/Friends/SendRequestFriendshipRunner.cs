using Runner.Interfaces;
using Runner.Notices;
using Services.Hubs;
using Services.Services;
using Services.ViewModels.HomeModels;

namespace Runner.Runners.Friends
{
    public class SendRequestFriendshipRunner : IRunner
    {
        public void Run(AccountViewModel account)
        {
            new FriendsService(new NoticesProxy()).SendRequestFriendship(account);
        }
    }
}
