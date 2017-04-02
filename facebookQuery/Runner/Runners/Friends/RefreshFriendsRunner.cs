﻿using Jobs.Notices;
using Runner.Interfaces;
using Services.Services;

namespace Runner.Runners.Friends
{
    public class RefreshFriendsRunner : IRunner
    {
        public void Run(IRunnerModel model)
        {
            var account = model.Account;

            var service = new FriendsService(new NoticesProxy());
            service.GetCurrentFriends(account);
        }
    }
}
