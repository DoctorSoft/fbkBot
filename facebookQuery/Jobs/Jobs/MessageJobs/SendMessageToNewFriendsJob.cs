﻿using System;
using Constants.FunctionEnums;
using Hangfire;
using Jobs.Interfaces;
using Jobs.JobsServices.BackgroundJobServices;
using Services.Models.BackgroundJobs;
using Services.Services;
using Services.ViewModels.JobStateViewModels;
using Services.ViewModels.QueueViewModels;

namespace Jobs.Jobs.MessageJobs
{
    public class SendMessageToNewFriendsJob : IRunJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public void Run(IRunJobModel runModel)
        {
            var account = runModel.Account;
            var forSpy = runModel.ForSpy;
            var friend = runModel.Friend;

            if (account.GroupSettingsId == null)
            {
                return;
            }
            
            var settings = new GroupService(new NoticeService()).GetSettings((long)account.GroupSettingsId);
            var sendNewFriendLaunchTime = new TimeSpan(settings.RetryTimeSendNewFriendHour, settings.RetryTimeSendNewFriendMin, settings.RetryTimeSendNewFriendSec);

            var model = new CreateBackgroundJobModel
            {
                Account = account,
                FunctionName = FunctionName.SendMessageToNewFriends,
                LaunchTime = sendNewFriendLaunchTime,
                CheckPermissions = true,
                IsForSpy = forSpy
            };

            new JobStateService().DeleteJobState(new JobStateViewModel
            {
                AccountId = account.Id,
                FunctionName = FunctionName.SendMessageToNewFriends,
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
                FunctionName = FunctionName.SendMessageToNewFriends,
                IsForSpy = forSpy
            });
        }
    }
}
