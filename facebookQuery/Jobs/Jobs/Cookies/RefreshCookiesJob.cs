using Constants.FunctionEnums;
using Services.Services;
using Services.ViewModels.HomeModels;

namespace Jobs.Jobs.Cookies
{
    public static class RefreshCookiesJob
    {
        public static void Run(AccountViewModel account)
        {
            new JobQueueService().AddToQueue(account.Id, FunctionName.RefreshCookies);
        }
    }
}
