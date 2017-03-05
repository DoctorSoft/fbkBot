using Runner.Interfaces;
using Runner.Notices;
using Services.Hubs;
using Services.Services;
using Services.ViewModels.HomeModels;
using NoticesProxy = Runner.Notices.NoticesProxy;

namespace Runner.Runners.Friends
{
    public class GetNewFriendsAndRecommendedRunner : IRunner
    {
        public void Run(AccountViewModel account)
        {
            new FriendsService(new NoticesProxy()).GetNewFriendsAndRecommended(account);
        }
    }
}
