using Runner.Interfaces;
using Runner.Notices;
using Services.Hubs;
using Services.Services;
using Services.ViewModels.HomeModels;

namespace Runner.Runners.Friends
{
    public class RefreshFriendsRunner : IRunner
    {
        public void Run(AccountViewModel account)
        {
            new FriendsService(new NoticesProxy()).GetFriendsOfFacebook(account);
        }
    }
}
