using System;
using Constants.FunctionEnums;
using Hangfire;
using Jobs.JobsService;
using Jobs.Notices;
using Services.Models.BackgroundJobs;
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
            if (account.GroupSettingsId == null)
            {
                return;
            }

            new JobStatusService().DeleteJobStatus(account.Id, FunctionName.SendMessageToNewFriends, null);

            var settings = new GroupService(new NoticesProxy()).GetSettings((long)account.GroupSettingsId);
            var sendNewFriendLaunchTime = new TimeSpan(settings.RetryTimeSendNewFriendHour, settings.RetryTimeSendNewFriendMin, settings.RetryTimeSendNewFriendSec);

            var model = new CreateBackgroundJobModel
            {
                Account = account,
                FunctionName = FunctionName.SendMessageToNewFriends,
                LaunchTime = sendNewFriendLaunchTime,
                CheckPermissions = true
            };

            new BackgroundJobService().CreateBackgroundJob(model);
            
            new JobQueueService().AddToQueue(account.Id, FunctionName.SendMessageToNewFriends);
        }
    }
}
