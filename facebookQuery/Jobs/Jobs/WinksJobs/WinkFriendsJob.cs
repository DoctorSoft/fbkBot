﻿using System;
using Constants.FunctionEnums;
using Hangfire;
using Jobs.Interfaces;
using Jobs.JobsServices.BackgroundJobServices;
using Services.Models.BackgroundJobs;
using Services.Services;
using Services.ViewModels.JobStateViewModels;
using Services.ViewModels.QueueViewModels;

namespace Jobs.Jobs.WinksJobs
{
    public class WinkFriendsJob : IRunJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public void Run(IRunJobModel runModel)
        {
            var account = runModel.Account;
            var friend = runModel.Friend;
            var forSpy = runModel.ForSpy;

            if (account.GroupSettingsId == null)
            {
                return;
            }
            
            var settings = new GroupService(new NoticeService()).GetSettings((long) account.GroupSettingsId);
            var winkFriendsLaunchTime = new TimeSpan(settings.RetryTimeForWinkFriendsHour, settings.RetryTimeForWinkFriendsMin, settings.RetryTimeForWinkFriendsSec);
            
            var model = new CreateBackgroundJobModel
            {
                Account = account,
                FunctionName = FunctionName.Wink,
                LaunchTime = winkFriendsLaunchTime,
                CheckPermissions = true,
                IsForSpy = forSpy
            };

            new JobStateService().DeleteJobState(new JobStateViewModel
            {
                AccountId = account.Id,
                FunctionName = FunctionName.Wink,
                IsForSpy = forSpy
            });

            var jobIsSuccessfullyCreated = new BackgroundJobService().CreateBackgroundJob(model);
            if (!jobIsSuccessfullyCreated)
            {
                return;
            }

            new JobQueueService().AddToQueue(new JobQueueViewModel
            {
                AccountId = account.Id,
                FunctionName = FunctionName.Wink,
                IsForSpy = forSpy
            });
        }
    }
}
