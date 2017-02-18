﻿using Runner.Interfaces;
using Services.Services;
using Services.ViewModels.HomeModels;

namespace Runner.Runners.Friends
{
    public class ConfirmFriendshipRunner : IRunner
    {
        public void Run(AccountViewModel account)
        {
            new FriendsService().ConfirmFriendship(account);
        }
    }
}