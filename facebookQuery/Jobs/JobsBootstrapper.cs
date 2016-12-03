using System;
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
            var accounts = new HomeService().GetAccounts();
            foreach (var accountViewModel in accounts)
            {
                RecurringJob.AddOrUpdate(string.Format("Respond to unread messages from {0}", accountViewModel.Login), () => SendMessageToUnreadJob.Run(accountViewModel), Cron.Minutely);
                RecurringJob.AddOrUpdate(string.Format("Respond to unanswered messages from {0}", accountViewModel.Login), () => SendMessageToUnansweredJob.Run(accountViewModel), Cron.Minutely);
                RecurringJob.AddOrUpdate(string.Format("Refresh friends list for account = {0} )", accountViewModel.Login), () => RefreshFriendsJob.Run(accountViewModel.FacebookId), "* 0/1 * * *");
            }
            //RecurringJob.AddOrUpdate(string.Format("Sending letters to new friends from = {0} )", account.Login), () => SendMessageToNewFriendsJob.Run(account), Cron.Minutely);
        }
    }
}
