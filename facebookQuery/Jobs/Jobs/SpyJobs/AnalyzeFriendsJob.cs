﻿using Hangfire;
using Jobs.JobsService;
using Services.Services;
using Services.ViewModels.HomeModels;

namespace Jobs.Jobs.SpyJobs
{
    public static class AnalyzeFriendsJob
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail), Queue("analyzefriends", Order = 1)]
        public static void Run(AccountViewModel account)
        {
            new SpyService(new JobService()).AnalyzeFriends(account);
        }
    }
}
