﻿using System;
using System.Linq;
using Hangfire;
using Jobs.Jobs.FriendJobs;
using Jobs.Jobs.MessageJobs;
using Services.Services;

namespace Jobs
{
    public static class JobsBootstrapper
    {
        public static void SetUpJobs()
        {
            //todo: uncomment it back
            var account = new HomeService().GetAccounts().FirstOrDefault();

            //RecurringJob.AddOrUpdate(string.Format("Refresh friends list for accountId = {0} )", account), () => RefreshFriendsJob.Run(account), "* 0/1 * * *");

            //RecurringJob.AddOrUpdate(string.Format("Receive unread messages for accountId = {0} )", account), () => GetUnreadMessagesJob.Run(account), "* 0/1 * * *");
            
            //var accounts = new HomeService().GetAccounts().Select(model => model.UserId).Take(2).ToList();
            RecurringJob.AddOrUpdate(string.Format("Response new posts from {0}", account), () => SendMessageJob.Run(account.UserId), Cron.Minutely);
            //RecurringJob.AddOrUpdate(string.Format("Send Message Job (from {0} to {1})", accounts[1], accounts[0]), () => SendMessageJob.Run(accounts[1], accounts[0]), Cron.Minutely);
        }
    }
}
