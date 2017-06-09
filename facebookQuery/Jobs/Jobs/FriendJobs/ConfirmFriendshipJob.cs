using System;
using Constants.FunctionEnums;
using Hangfire;
using Jobs.JobsService;
using Jobs.Models;
using Services.Models.BackgroundJobs;
using Services.Services;
using Services.ViewModels.JobStateViewModels;
using Services.ViewModels.QueueViewModels;

namespace Jobs.Jobs.FriendJobs
{
    public static class ConfirmFriendshipJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public static void Run(RunJobModel runModel)
        {
            var account = runModel.Account;
            var forSpy = runModel.ForSpy;
            var friend = runModel.Friend;

            if (account.GroupSettingsId == null)
            {
                return;
            }

            var settings = new GroupService(new NoticeService()).GetSettings((long) account.GroupSettingsId);
            var confirmFriendshipsLaunchTime = new TimeSpan(settings.RetryTimeConfirmFriendshipsHour, settings.RetryTimeConfirmFriendshipsMin, settings.RetryTimeConfirmFriendshipsSec);

            var model = new CreateBackgroundJobModel
            {
                Account = account,
                FunctionName = FunctionName.ConfirmFriendship,
                LaunchTime = confirmFriendshipsLaunchTime,
                CheckPermissions = true,
                IsForSpy = forSpy
            };

            new JobStateService().DeleteJobState(new JobStateViewModel
            {
                AccountId = account.Id,
                FunctionName = FunctionName.ConfirmFriendship,
                IsForSpy = forSpy
            });

            var jobIsSuccessfullyCreated = new BackgroundJobService().CreateBackgroundJob(model);
            if (!jobIsSuccessfullyCreated)
            {
                return;
            }

            new JobQueueService().AddToQueue(new JobQueueViewModel
            {
                AccountId = account.Id,
                FunctionName = FunctionName.ConfirmFriendship,
                IsForSpy = forSpy
            });
        }
    }
}
