using Hangfire;
using Jobs.Jobs.Cookies;
using Jobs.Jobs.FriendJobs;
using Jobs.Jobs.MessageJobs;
using Jobs.Jobs.RunnerJob;
using Jobs.Jobs.SpyJobs;
using Services.Interfaces;
using Services.Services;
using Services.ViewModels.HomeModels;

namespace Jobs.JobsService
{
    public class JobService : IJobService
    {
        private readonly JobStatusService _jobStatusService;
        
        const string UnreadMessagesPattern = "Respond to unread messages from {0}";
        const string UnansweredMessagesPattern = "Respond to unanswered messages from {0}";
        const string NewFriendMessagesPattern = "Send message to new friend from {0}";
        const string RefreshFriendsPattern = "Refresh friends list for account = {0}";
        const string AddNewFriendsPattern = "Add new friends and recommended for account = {0}";
        const string AnalyzeFriendsPattern = "Analyze spy friends account = {0}";
        const string ConfirmFriendshipPattern = "Confirm friendship for account = {0}";
        const string SendRequestFriendshipPattern = "Send request friendship for account = {0}";
        const string RefreshCookiesPattern = "Refresh cookies for account = {0}";
        const string RunnerPattern = "Runner for account = {0}";

        public JobService()
        {
            _jobStatusService = new JobStatusService();
        }

        public void AddOrUpdateAccountJobs(AccountViewModel accountViewModel)
        {
            RecurringJob.AddOrUpdate(string.Format(RefreshCookiesPattern, accountViewModel.Login), () => RefreshCookiesJob.Run(accountViewModel), Cron.Hourly);
            RecurringJob.AddOrUpdate(string.Format(UnreadMessagesPattern, accountViewModel.Login), () => SendMessageToUnreadJob.Run(accountViewModel), Cron.Minutely);
            RecurringJob.AddOrUpdate(string.Format(UnansweredMessagesPattern, accountViewModel.Login), () => SendMessageToUnansweredJob.Run(accountViewModel), Cron.Minutely);
            RecurringJob.AddOrUpdate(string.Format(NewFriendMessagesPattern, accountViewModel.Login), () => SendMessageToNewFriendsJob.Run(accountViewModel), Cron.Minutely);
            RecurringJob.AddOrUpdate(string.Format(RefreshFriendsPattern, accountViewModel.Login), () => RefreshFriendsJob.Run(accountViewModel), Cron.Minutely);
            RecurringJob.AddOrUpdate(string.Format(AddNewFriendsPattern, accountViewModel.Login), () => GetNewFriendsAndRecommendedJob.Run(accountViewModel), Cron.Minutely);
            RecurringJob.AddOrUpdate(string.Format(ConfirmFriendshipPattern, accountViewModel.Login), () => ConfirmFriendshipJob.Run(accountViewModel), Cron.Minutely);
            RecurringJob.AddOrUpdate(string.Format(SendRequestFriendshipPattern, accountViewModel.Login), () => SendRequestFriendshipJob.Run(accountViewModel), Cron.Minutely);
            RecurringJob.AddOrUpdate(string.Format(RunnerPattern, accountViewModel.Login), () => RunnerJob.Run(accountViewModel), Cron.Minutely);
        }

        public void AddOrUpdateSpyAccountJobs(AccountViewModel accountViewModel)
        {
            //for add or update spy only account
            RecurringJob.AddOrUpdate(string.Format(AnalyzeFriendsPattern, accountViewModel.Login), () => AnalyzeFriendsJob.Run(accountViewModel), Cron.Minutely);
        }

        public void RemoveAccountJobs(string login, long? accountId)
        {
            RecurringJob.RemoveIfExists(string.Format(UnreadMessagesPattern, login));
            RecurringJob.RemoveIfExists(string.Format(UnansweredMessagesPattern, login));
            RecurringJob.RemoveIfExists(string.Format(NewFriendMessagesPattern, login));
            RecurringJob.RemoveIfExists(string.Format(RefreshFriendsPattern, login));
            RecurringJob.RemoveIfExists(string.Format(AddNewFriendsPattern, login));
            RecurringJob.RemoveIfExists(string.Format(AnalyzeFriendsPattern, login));
            RecurringJob.RemoveIfExists(string.Format(ConfirmFriendshipPattern, login));
            RecurringJob.RemoveIfExists(string.Format(SendRequestFriendshipPattern, login));
            RecurringJob.RemoveIfExists(string.Format(RefreshCookiesPattern, login));

            if (accountId != null)
            {
                _jobStatusService.DeleteJobStatuses((long)accountId);
            }
        }

        public void RenameAccountJobs(AccountViewModel accountViewModel, string oldLogin)
        {
            RemoveAccountJobs(oldLogin, accountViewModel.Id);
            AddOrUpdateAccountJobs(accountViewModel);
        }
    }
}
