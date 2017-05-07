using Constants.FunctionEnums;
using Hangfire;
using Jobs.Models;
using Services.Services;
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

            var jobQueueModel = new JobQueueViewModel
            {
                AccountId = account.Id,
                FunctionName = FunctionName.CheckFriendsAtTheEndTimeConditions,
                IsForSpy = forSpy
            };

            new JobQueueService().AddToQueue(jobQueueModel);
        }
    }
}
