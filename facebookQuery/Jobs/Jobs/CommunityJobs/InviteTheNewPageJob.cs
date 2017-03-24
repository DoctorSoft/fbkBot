using System;
using Constants.FunctionEnums;
using Hangfire;
using Jobs.JobsService;
using Jobs.Notices;
using Services.Models.BackgroundJobs;
using Services.Services;
using Services.ServiceTools;
using Services.ViewModels.HomeModels;

namespace Jobs.Jobs.CommunityJobs
{
    public static class InviteTheNewPageJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public static void Run(AccountViewModel account)
        {
            if (account.GroupSettingsId == null)
            {
                return;
            }

            if (!new FunctionPermissionManager().HasPermissions(FunctionName.InviteToPages, (long)account.GroupSettingsId))
            {
                return;
            }

            new JobStatusService().DeleteJobStatus(account.Id, FunctionName.InviteToPages, null);
            
            var settings = new GroupService(new NoticesProxy()).GetSettings((long)account.GroupSettingsId);
            var inviteTheNewPageLaunchTime = new TimeSpan(settings.RetryTimeInviteThePagesHour, settings.RetryTimeInviteThePagesMin, settings.RetryTimeInviteThePagesSec);

            var model = new CreateBackgroundJobModel
            {
                Account = account,
                FunctionName = FunctionName.InviteToPages,
                LaunchTime = inviteTheNewPageLaunchTime,
                CheckPermissions = true
            };

            new BackgroundJobService().CreateBackgroundJob(model);
            
            new JobQueueService().AddToQueue(account.Id, FunctionName.InviteToPages);
        }
    }
}
