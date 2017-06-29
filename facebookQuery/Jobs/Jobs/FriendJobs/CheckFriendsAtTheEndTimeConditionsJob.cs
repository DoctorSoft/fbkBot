using Constants.FunctionEnums;
using Hangfire;
using Jobs.AbstractJobs;
using Jobs.JobsServices.JobServices;
using Jobs.Models;
using Services.Models.Jobs;
using Services.Services;
using Services.ServiceTools;
using Services.ViewModels.QueueViewModels;

namespace Jobs.Jobs.FriendJobs
{
    public static class CheckFriendsAtTheEndTimeConditionsJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public static void Run(RunJobModel runModel)
        {
            var account = runModel.Account;
            var forSpy = runModel.ForSpy;


            if (account.GroupSettingsId == null)
            {
                return;
            }
            if (new AccountManager().GetAccountById(account.Id) == null)
            {
                new JobService().RemoveAccountJobs(new RemoveAccountJobsModel
                {
                    AccountId = account.Id,
                    IsForSpy = forSpy,
                    Login = account.Login
                });

                return;
            }

            if (!new AccountManager().HasAWorkingAccount(account.Id))
            {
                return;
            }

            new JobQueueService().AddToQueue(new JobQueueViewModel
            {
                AccountId = account.Id,
                FunctionName = FunctionName.CheckFriendsAtTheEndTimeConditions,
                IsForSpy = forSpy
            });
        }
    }
}
