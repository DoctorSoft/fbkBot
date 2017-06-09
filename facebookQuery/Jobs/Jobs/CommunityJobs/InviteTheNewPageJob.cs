using System;
using Constants.FunctionEnums;
using Hangfire;
using Jobs.JobsService;
using Jobs.Models;
using Services.Models.BackgroundJobs;
using Services.Services;
using Services.ServiceTools;
using Services.ViewModels.JobStateViewModels;
using Services.ViewModels.JobStatusModels;
using Services.ViewModels.QueueViewModels;

namespace Jobs.Jobs.CommunityJobs
{
    public static class InviteTheNewPageJob
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

            if (!new FunctionPermissionManager().HasPermissions(FunctionName.InviteToPages, (long)account.GroupSettingsId))
            {
                return;
            }
            
            var settings = new GroupService(new NoticeService()).GetSettings((long)account.GroupSettingsId);
            var inviteTheNewPageLaunchTime = new TimeSpan(settings.RetryTimeInviteThePagesHour, settings.RetryTimeInviteThePagesMin, settings.RetryTimeInviteThePagesSec);

            var model = new CreateBackgroundJobModel
            {
                Account = account,
                FunctionName = FunctionName.InviteToPages,
                LaunchTime = inviteTheNewPageLaunchTime,
                CheckPermissions = true,
                IsForSpy = forSpy
            };

            new JobStateService().DeleteJobState(new JobStateViewModel
            {
                AccountId = account.Id,
                FunctionName = FunctionName.InviteToPages,
                FriendId = friend.Id,
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
                FunctionName = FunctionName.InviteToPages,
                FriendId = friend.Id,
                IsForSpy = forSpy
            });
        }
    }
}
