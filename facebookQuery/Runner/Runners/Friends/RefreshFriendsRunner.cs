using Runner.Interfaces;
using Runner.Notices;
using Services.Services;
using Services.ViewModels.HomeModels;

namespace Runner.Runners.Friends
{
    public class RefreshFriendsRunner : IRunner
    {
        public void Run(AccountViewModel account)
        {
            var service = new FriendsService(new NoticesProxy());
            service.GetFriendsOfFacebook(account);
        }
    }
}
