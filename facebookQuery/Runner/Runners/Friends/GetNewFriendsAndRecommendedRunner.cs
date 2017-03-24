using Jobs.JobsService;
using Jobs.Notices;
using Runner.Interfaces;
using Services.Services;
using Services.ViewModels.HomeModels;

namespace Runner.Runners.Friends
{
    public class GetNewFriendsAndRecommendedRunner : IRunner
    {
        public void Run(AccountViewModel account)
        {
            new FriendsService(new NoticesProxy()).GetNewFriendsAndRecommended(account, new BackgroundJobService());
        }
    }
}
