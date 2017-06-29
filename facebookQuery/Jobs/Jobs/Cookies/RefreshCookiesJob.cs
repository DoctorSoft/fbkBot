using Constants.FunctionEnums;
using Jobs.Models;
using Services.Services;
using Services.ViewModels.QueueViewModels;

namespace Jobs.Jobs.Cookies
{
    public static class RefreshCookiesJob
    {
        public static void Run(RunJobModel runModel)
        {
            var account = runModel.Account;
            var forSpy = runModel.ForSpy;
            
            new JobQueueService().AddToQueue(new JobQueueViewModel
            {
                AccountId = account.Id,
                FunctionName = FunctionName.RefreshCookies,
                IsForSpy = forSpy
            });
        }
    }
}
