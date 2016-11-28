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
            //var account = new HomeService().GetAccounts().FirstOrDefault();

            //RecurringJob.AddOrUpdate(string.Format("Refresh friends list for account = {0} )", account.Login), () => RefreshFriendsJob.Run(account.FacebookId), "* 0/1 * * *");
            //RecurringJob.AddOrUpdate(string.Format("Sending letters to new friends from = {0} )", account.Login), () => SendMessageToNewFriendsJob.Run(account), Cron.Minutely);

            //RecurringJob.AddOrUpdate(string.Format("Receive unread messages for accountId = {0} )", account), () => GetUnreadMessagesJob.Run(account), "* 0/1 * * *");
            
            //var accounts = new HomeService().GetAccounts().Select(model => model.UserId).Take(2).ToList();
            //RecurringJob.AddOrUpdate(string.Format("Respond to unread messages from {0}", account.Login), () => SendMessageToUnreadJob.Run(account), Cron.Minutely);
            //RecurringJob.AddOrUpdate(string.Format("Respond to unanswered messages from {0}", account.Login), () => SendMessageToUnansweredJob.Run(account), Cron.Minutely);
        }
    }
}
