using System;
using Constants.FunctionEnums;
using Hangfire;
using Jobs.JobsService;
using Jobs.Notices;
using Services.Models.BackgroundJobs;
using Services.Services;
using Services.ViewModels.HomeModels;

namespace Jobs.Jobs.FriendJobs
{
    public static class ConfirmFriendshipJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public static void Run(AccountViewModel account)
        {
            if (account.GroupSettingsId == null)
            {
                return;
            }

            new JobStatusService().DeleteJobStatus(account.Id, FunctionName.ConfirmFriendship, null);

            var settings = new GroupService(new NoticesProxy()).GetSettings((long) account.GroupSettingsId);
            var confirmFriendshipsLaunchTime = new TimeSpan(settings.RetryTimeConfirmFriendshipsHour, settings.RetryTimeConfirmFriendshipsMin, settings.RetryTimeConfirmFriendshipsSec);

            var model = new CreateBackgroundJobModel
            {
                Account = account,
                FunctionName = FunctionName.ConfirmFriendship,
                LaunchTime = confirmFriendshipsLaunchTime,
                CheckPermissions = true
            };

            new BackgroundJobService().CreateBackgroundJob(model);
            
            new JobQueueService().AddToQueue(account.Id, FunctionName.ConfirmFriendship);
        }
    }
}
