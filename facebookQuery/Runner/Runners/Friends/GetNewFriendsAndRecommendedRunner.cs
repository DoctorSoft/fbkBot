﻿using Jobs.JobsService;

using Runner.Interfaces;
using Services.Services;

namespace Runner.Runners.Friends
{
    public class GetNewFriendsAndRecommendedRunner : IRunner
    {
        public void Run(IRunnerModel model)
        {
            var account = model.Account;

            new FriendsService(new NoticeService()).GetNewFriendsAndRecommended(account, new BackgroundJobService());
        }
    }
}
