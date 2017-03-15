using System;
using Constants.FunctionEnums;
using Hangfire;
using Jobs.JobsService;
using Runner.Notices;
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

            new JobStatusService().DeleteJobStatus(account.Id, FunctionName.InviteToGroups);

            var settings = new GroupService(new NoticesProxy()).GetSettings((long)account.GroupSettingsId);
            var inviteTheNewGroupLaunchTime = new TimeSpan(settings.RetryTimeInviteTheGroupsHour, settings.RetryTimeInviteTheGroupsMin, settings.RetryTimeInviteTheGroupsSec);
            new BackgroundJobService().CreateBackgroundJob(account, FunctionName.InviteToGroups, inviteTheNewGroupLaunchTime, true);


            new JobQueueService().AddToQueue(account.Id, FunctionName.InviteToGroups);
        }
    }
}
