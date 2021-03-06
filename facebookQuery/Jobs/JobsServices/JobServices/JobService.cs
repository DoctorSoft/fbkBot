﻿using CommonInterfaces.Interfaces.Models;
using CommonInterfaces.Interfaces.Services.Jobs;
using Hangfire;
using Jobs.Jobs.Cookies;
using Jobs.Jobs.FriendJobs;
using Jobs.Jobs.Notices;
using Jobs.Jobs.Runners;
using Jobs.Jobs.SpyJobs;
using Jobs.Models;
using Services.Models.Jobs;
using Services.ViewModels.HomeModels;
using AddOrUpdateAccountModel = Services.Models.BackgroundJobs.AddOrUpdateAccountModel;

namespace Jobs.JobsServices.JobServices
{
    public class JobService : IJobService
    {
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
        const string InviteTheNewGroupPattern = "Invite the new group = {0}";
        const string CheckFriendsConditionsToRemovePattern = "Check conditions to remove = {0}";
        const string AddToScheduleDeleteFromFriends = "Add to schedule delete from friends = {0}";
        const string ClearOldNoticesPattern = "Clear old notices";
        const string DeleteOldRunnersPattern = "Remove old runner records";

        public void AddOrUpdateGeneralJobs()
        {
            RecurringJob.AddOrUpdate(ClearOldNoticesPattern, () => ClearNoticesJob.Run(), Cron.Minutely);
            RecurringJob.AddOrUpdate(DeleteOldRunnersPattern, () => RemoveOldRunnersJob.Run(), Cron.Minutely);
        }

        public void AddOrUpdateAccountJobs(IAddOrUpdateAccountJobs model)
        {
            var currentModel = model as AddOrUpdateAccountModel;

            if (currentModel == null)
            {
                return;
            }

            var accountViewModel = currentModel.Account;
            var runModel = new RunJobModel
            {
                Account = accountViewModel
            };

            RecurringJob.AddOrUpdate(string.Format(CheckFriendsConditionsToRemovePattern, accountViewModel.Login), () => CheckFriendsAtTheEndTimeConditionsJob.Run(runModel), "*/30 * * * *");
            RecurringJob.AddOrUpdate(string.Format(RefreshCookiesPattern, accountViewModel.Login), () => RefreshCookiesJob.Run(runModel), Cron.Daily);

            /*RecurringJob.AddOrUpdate(string.Format(InviteTheNewGroupPattern, accountViewModel.Login), () => InviteTheNewGroupJob.Run(accountViewModel), Cron.Hourly);
           RecurringJob.AddOrUpdate(string.Format(RefreshCookiesPattern, accountViewModel.Login), () => RefreshCookiesJob.Run(accountViewModel), Cron.Hourly);
           RecurringJob.AddOrUpdate(string.Format(UnreadMessagesPattern, accountViewModel.Login), () => SendMessageToUnreadJob.Run(accountViewModel), Cron.Minutely);
           RecurringJob.AddOrUpdate(string.Format(UnansweredMessagesPattern, accountViewModel.Login), () => SendMessageToUnansweredJob.Run(accountViewModel), Cron.Minutely);
           RecurringJob.AddOrUpdate(string.Format(NewFriendMessagesPattern, accountViewModel.Login), () => SendMessageToNewFriendsJob.Run(accountViewModel), Cron.Minutely);
           RecurringJob.AddOrUpdate(string.Format(RefreshFriendsPattern, accountViewModel.Login), () => RefreshFriendsJob.Run(accountViewModel), Cron.Minutely);
           RecurringJob.AddOrUpdate(string.Format(AddNewFriendsPattern, accountViewModel.Login), () => GetNewFriendsAndRecommendedJob.Run(accountViewModel), Cron.Minutely);
           RecurringJob.AddOrUpdate(string.Format(ConfirmFriendshipPattern, accountViewModel.Login), () => ConfirmFriendshipJob.Run(accountViewModel), Cron.Minutely);
           RecurringJob.AddOrUpdate(string.Format(SendRequestFriendshipPattern, accountViewModel.Login), () => SendRequestFriendshipJob.Run(accountViewModel), Cron.Minutely);
           RecurringJob.AddOrUpdate(string.Format(RunnerPattern, accountViewModel.Login), () => RunnerJob.Run(accountViewModel), Cron.Minutely);*/

        }

        public void AddOrUpdateSpyAccountJobs(IAddOrUpdateAccountJobs model)
        {
            var currentModel = model as AddOrUpdateAccountModel;

            if (currentModel == null)
            {
                return;
            }
            var accountViewModel = currentModel.Account;

            RecurringJob.AddOrUpdate(string.Format(RefreshCookiesPattern, accountViewModel.Login), () => RefreshCookiesJob.Run(new RunJobModel { ForSpy = true, Account = accountViewModel }), Cron.Daily);

            if (AccountIsWorking(accountViewModel))
            {
                //for add or update spy only account
                RecurringJob.AddOrUpdate(string.Format(AnalyzeFriendsPattern, accountViewModel.Login), () => AnalyzeFriendsJob.Run(accountViewModel), Cron.Minutely);
            }
        }

        public void RemoveAccountJobs(IRemoveAccountJobs model)
        {
            var currentModel = model as RemoveAccountJobsModel;

            if (currentModel == null)
            {
                return;
            }

            var login = currentModel.Login;

            RecurringJob.RemoveIfExists(string.Format(UnreadMessagesPattern, login));
            RecurringJob.RemoveIfExists(string.Format(UnansweredMessagesPattern, login));
            RecurringJob.RemoveIfExists(string.Format(NewFriendMessagesPattern, login));
            RecurringJob.RemoveIfExists(string.Format(RefreshFriendsPattern, login));
            RecurringJob.RemoveIfExists(string.Format(AddNewFriendsPattern, login));
            RecurringJob.RemoveIfExists(string.Format(AnalyzeFriendsPattern, login));
            RecurringJob.RemoveIfExists(string.Format(ConfirmFriendshipPattern, login));
            RecurringJob.RemoveIfExists(string.Format(SendRequestFriendshipPattern, login));
            RecurringJob.RemoveIfExists(string.Format(RefreshCookiesPattern, login));
            RecurringJob.RemoveIfExists(string.Format(CheckFriendsConditionsToRemovePattern, login));
        }
      
        public void RenameAccountJobs(IRenameAccountJobs model)
        {
            var currentModel = model as RenameAccountJobsModel;

            if (currentModel == null)
            {
                return;
            }

            var accountViewModel = currentModel.AccountViewModel;
            var oldLogin = currentModel.OldLogin;

            var removeAccountModel = new RemoveAccountJobsModel
            {
                AccountId = accountViewModel.Id,
                Login = oldLogin
            };

            var addOrUpdateAccountModel = new AddOrUpdateAccountModel
            {
                Account = accountViewModel
            };

            RemoveAccountJobs(removeAccountModel);
            AddOrUpdateAccountJobs(addOrUpdateAccountModel);
        }

        private static bool AccountIsWorking(AccountViewModel account)
        {
            if (account.AuthorizationDataIsFailed || account.ProxyDataIsFailed || account.IsDeleted || account.ConformationDataIsFailed)
            {
                return false;
            }

            return true;
        }
    }

}
