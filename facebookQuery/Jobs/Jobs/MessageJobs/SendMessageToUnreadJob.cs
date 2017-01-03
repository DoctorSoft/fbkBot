using Constants.FunctionEnums;
using Hangfire;
using Services.Services;
using Services.ServiceTools;
using Services.ViewModels.HomeModels;

namespace Jobs.Jobs.MessageJobs
{
    public static class SendMessageToUnreadJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail), Queue("sendmessagetounread", Order = 1)]
        public static void Run(AccountViewModel account)
        {
            if (!new FunctionPermissionManager().HasPermissionsByFacebookId(FunctionName.SendMessageToUnread, account.FacebookId))
            {
                return;
            }

            new FacebookMessagesService().SendMessageToUnread(account);
        }
    }
}
