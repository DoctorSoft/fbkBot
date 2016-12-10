using Constants.FunctionEnums;
using Hangfire;
using Services.Services;
using Services.ServiceTools;
using Services.ViewModels.HomeModels;

namespace Jobs.Jobs.MessageJobs
{
    public static class SendMessageToNewFriendsJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public static void Run(AccountViewModel account)
        {
            if (!new FunctionPermissionManager().HasPermissionsByFacebookId(FunctionName.SendMessageToNewFriends, account.FacebookId))
            {
                return;
            }

            new FacebookMessagesService().SendMessageToNewFriends(account);
        }
    }
}
