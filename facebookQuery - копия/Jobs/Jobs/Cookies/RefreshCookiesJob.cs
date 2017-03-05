using System;
using Constants.FunctionEnums;
using Jobs.JobsService;
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

            new BackgroundJobService().CreateBackgroundJob(account, FunctionName.RefreshCookies, new TimeSpan(2, 0, 0));

            new JobStatusService().AddOrUpdateJobStatus(FunctionName.RefreshCookies, account.Id);

            new JobQueueService().AddToQueue(account.Id, FunctionName.RefreshCookies);
        }
    }
}
