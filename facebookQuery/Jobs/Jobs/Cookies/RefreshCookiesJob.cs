using System;
using Constants.FunctionEnums;
using Jobs.JobsService;
using Services.Models.BackgroundJobs;
using Services.Services;
using Services.ViewModels.HomeModels;

namespace Jobs.Jobs.Cookies
{
    public static class RefreshCookiesJob
    {
        public static void Run(AccountViewModel account)
        {
            if (account.GroupSettingsId == null)
            {
                return;
            }

            new JobStatusService().DeleteJobStatus(account.Id, FunctionName.RefreshCookies, null);

            var model = new CreateBackgroundJobModel
            {
                Account = account,
                FunctionName = FunctionName.RefreshCookies,
                LaunchTime = new TimeSpan(2, 0, 0),
                CheckPermissions = false
            };

            new BackgroundJobService().CreateBackgroundJob(model);
            
            new JobQueueService().AddToQueue(account.Id, FunctionName.RefreshCookies);
        }
    }
}
