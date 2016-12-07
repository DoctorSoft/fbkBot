﻿using System;
using System.Linq;
using Hangfire;
using Jobs.Jobs.FriendJobs;
using Jobs.Jobs.MessageJobs;
using Jobs.JobsService;
using Services.Services;
using Services.ServiceTools;

namespace Jobs
{
    public static class JobsBootstrapper
    {
        public static void SetUpJobs()
        {
            //todo: uncomment it back
            var accounts = new HomeService(new JobService(), new AccountManager()).GetAccounts();
            foreach (var accountViewModel in accounts)
            {
                new JobService().AddOrUpdateAccountJobs(accountViewModel);
            }
            //RecurringJob.AddOrUpdate(string.Format("Sending letters to new friends from = {0} )", account.Login), () => SendMessageToNewFriendsJob.Run(account), Cron.Minutely);
        }
    }
}
