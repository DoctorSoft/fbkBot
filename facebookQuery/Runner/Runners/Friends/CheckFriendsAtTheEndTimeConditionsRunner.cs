﻿
using Runner.Interfaces;
using Services.Services;

namespace Runner.Runners.Friends
{
    public class CheckFriendsAtTheEndTimeConditionsRunner : IRunner
    {
        public void Run(IRunnerModel model)
        {
            var account = model.Account;

            new FriendsService(new NoticeService()).CheckFriendsAtTheEndTimeConditions(account);
        }
    }
}
