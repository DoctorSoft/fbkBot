﻿using Runner.Interfaces;
using Services.Services;

namespace Runner.Runners.Friends
{
    public class SendRequestFriendshipRunner : IRunner
    {
        public void Run(IRunnerModel model)
        {
            var account = model.Account;
            new FriendsService(new NoticeService()).SendRequestFriendship(account);
        }
    }
}
