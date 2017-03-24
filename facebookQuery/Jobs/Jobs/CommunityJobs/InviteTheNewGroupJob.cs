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
    public static class InviteTheNewGroupJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public static void Run(AccountViewModel account)
        {
            if (account.GroupSettingsId == null)
            {
                return;
            }

            if (!new FunctionPermissionManager().HasPermissions(FunctionName.InviteToGroups, (long)account.GroupSettingsId))
            {
                return;
            }

            new JobStatusService().DeleteJobStatus(account.Id, FunctionName.InviteToGroups, null);

            var settings = new GroupService(new NoticesProxy()).GetSettings((long)account.GroupSettingsId);
            var inviteTheNewGroupLaunchTime = new TimeSpan(settings.RetryTimeInviteTheGroupsHour, settings.RetryTimeInviteTheGroupsMin, settings.RetryTimeInviteTheGroupsSec);

            var model = new CreateBackgroundJobModel
            {
                Account = account,
                FunctionName = FunctionName.InviteToGroups,
                LaunchTime = inviteTheNewGroupLaunchTime,
                CheckPermissions = true
            };

            new BackgroundJobService().CreateBackgroundJob(model);
            
            new JobQueueService().AddToQueue(account.Id, FunctionName.InviteToGroups);
        }
    }
}
