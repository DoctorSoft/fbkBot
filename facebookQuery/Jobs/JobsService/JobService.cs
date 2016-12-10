using Hangfire;
using Jobs.Jobs.FriendJobs;
using Jobs.Jobs.MessageJobs;
using Services.Interfaces;
using Services.ViewModels.HomeModels;

namespace Jobs.JobsService
{
    public class JobService : IJobService
    {
        const string UnreadMessagesPattern = "Respond to unread messages from {0}";
        const string UnansweredMessagesPattern = "Respond to unanswered messages from {0}";
        const string NewFriendMessagesPattern = "Send message to new friend from {0}";
        const string RefreshFriendsPattern = "Refresh friends list for account = {0}";

        public void AddOrUpdateAccountJobs(AccountViewModel accountViewModel)
        {
            RecurringJob.AddOrUpdate(string.Format(UnreadMessagesPattern, accountViewModel.Login), () => SendMessageToUnreadJob.Run(accountViewModel), Cron.Minutely);
            RecurringJob.AddOrUpdate(string.Format(UnansweredMessagesPattern, accountViewModel.Login), () => SendMessageToUnansweredJob.Run(accountViewModel), Cron.Minutely);
            RecurringJob.AddOrUpdate(string.Format(NewFriendMessagesPattern, accountViewModel.Login), () => SendMessageToNewFriendsJob.Run(accountViewModel), Cron.Minutely);
            RecurringJob.AddOrUpdate(string.Format(RefreshFriendsPattern, accountViewModel.Login), () => RefreshFriendsJob.Run(accountViewModel.FacebookId), "* 0/1 * * *");
        }

        public void RemoveAccountJobs(string login)
        {
            RecurringJob.RemoveIfExists(string.Format(UnreadMessagesPattern, login));
            RecurringJob.RemoveIfExists(string.Format(UnansweredMessagesPattern, login));
            RecurringJob.RemoveIfExists(string.Format(NewFriendMessagesPattern, login));
            RecurringJob.RemoveIfExists(string.Format(RefreshFriendsPattern, login));
        }

        public void RenameAccountJobs(AccountViewModel accountViewModel, string oldLogin)
        {
            RemoveAccountJobs(oldLogin);
            AddOrUpdateAccountJobs(accountViewModel);
        }
    }
}
