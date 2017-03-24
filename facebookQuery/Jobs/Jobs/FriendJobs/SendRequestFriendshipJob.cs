using System;
using Constants.FunctionEnums;
using Hangfire;
using Jobs.JobsService;
using Jobs.Notices;
using Services.Models.BackgroundJobs;
using Services.Services;
using Services.ServiceTools;
using Services.ViewModels.HomeModels;

namespace Jobs.Jobs.FriendJobs
{
    public static class SendRequestFriendshipJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)] 
        public static void Run(AccountViewModel account)
        {
            if (account.GroupSettingsId == null)
            {
                return;
            }

            new JobStatusService().DeleteJobStatus(account.Id, FunctionName.SendRequestFriendship, null);

            var settings = new GroupService(new NoticesProxy()).GetSettings((long)account.GroupSettingsId);
            var sendRequestFriendshipsLaunchTime = new TimeSpan(settings.RetryTimeSendRequestFriendshipsHour, settings.RetryTimeSendRequestFriendshipsMin, settings.RetryTimeSendRequestFriendshipsSec);
            
            var model = new CreateBackgroundJobModel
            {
                Account = account,
                FunctionName = FunctionName.SendRequestFriendship,
                LaunchTime = sendRequestFriendshipsLaunchTime,
                CheckPermissions = true
            };

            new BackgroundJobService().CreateBackgroundJob(model);
            
            new JobQueueService().AddToQueue(account.Id, FunctionName.SendRequestFriendship);
        }
    }
}
