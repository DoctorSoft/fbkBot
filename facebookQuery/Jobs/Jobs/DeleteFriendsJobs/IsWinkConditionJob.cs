﻿using Constants.FunctionEnums;
using Hangfire;
using Services.Services;
using Services.ViewModels.FriendsModels;
using Services.ViewModels.HomeModels;

namespace Jobs.Jobs.DeleteFriendsJobs
{
    public static class IsWinkConditionJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public static void Run(AccountViewModel account, FriendViewModel friend)
        {
            if (account.GroupSettingsId == null)
            {
                return;
            }

            new JobStatusService().DeleteJobStatus(account.Id, FunctionName.IsWink, friend.Id);

            new JobQueueService().AddToQueue(account.Id, FunctionName.IsWink);
        }
    }
}
