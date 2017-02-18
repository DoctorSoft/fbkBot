using Constants.FunctionEnums;
using Services.ServiceTools;
using Services.ViewModels.HomeModels;

namespace Jobs.Jobs.Cookies
{
    public static class RefreshCookiesJob
    {
        public static void Run(AccountViewModel account)
        {
            //var jobStatusService = new JobStatusService();

            //jobStatusService.AddOrUpdateStatus(account.Id, JobNames.RefreshCookies.GetDiscription());

            new JobQueueService().AddToQueue(account.Id, FunctionName.RefreshCookies);
            //jobStatusService.AddOrUpdateStatus(account.Id, JobNames.RefreshCookies.GetDiscription());
            
        }
    }
}
