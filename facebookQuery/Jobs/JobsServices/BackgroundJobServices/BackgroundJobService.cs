using System;
using System.Collections.Generic;
using System.Linq;
using CommonInterfaces.Interfaces.Models;
using CommonInterfaces.Interfaces.Services.BackgroundJobs;
using CommonModels;
using Constants.FunctionEnums;
using Hangfire;
using Jobs.Models;
using Services.Models.BackgroundJobs;
using Services.Models.Jobs;
using Services.Services;
using Services.ServiceTools;
using Services.ViewModels.GroupModels;
using Services.ViewModels.HomeModels;
using Services.ViewModels.JobStateViewModels;
using AddOrUpdateAccountModel = Services.Models.BackgroundJobs.AddOrUpdateAccountModel;

namespace Jobs.JobsServices.BackgroundJobServices
{
    public class BackgroundJobService : IBackgroundJobService
    {
        private readonly JobStateService _jobStateService;
        private readonly GroupFunctionsService _groupFunctionsService;
        private readonly List<FunctionName> _requaredFunctions;
        private readonly AccountManager _accountManager;


        public BackgroundJobService()
        {
            _groupFunctionsService = new GroupFunctionsService();
            _jobStateService = new JobStateService();
            _requaredFunctions = new RequiredFunctions().GetRequiredFunctions();
            _accountManager = new AccountManager();
        }

        public void RemoveAllBackgroundJobs(IRemoveAccountJobs model)
        {
            var currentModel = model as RemoveAccountJobsModel;

            var accountId = currentModel?.AccountId;

            if (accountId == null)
            {
                return;
            }
            
            var jobsId = _jobStateService.DeleteJobStateByAccount((long)accountId, currentModel.IsForSpy);

            RemoveJobsById(jobsId);
        }

        public void RemoveBackgroundJobById(string jobId)
        {
            try
            {
                _jobStateService.DeleteJobStateByJobId(jobId);

                BackgroundJob.Delete(jobId);
            }
            catch (Exception) { }
        }

        private void RemoveJobsById(IEnumerable<string> jobsId)
        {
            foreach (var jobId in jobsId)
            {
                try
                {
                    BackgroundJob.Delete(jobId);
                }
                catch (Exception) { }
            }
        }

        public bool AddOrUpdateAccountJobs(IAddOrUpdateAccountJobs model)
        {
            var currentModel = (AddOrUpdateAccountModel) model;

            if (currentModel == null)
            {
                return false;
            }
            var account = currentModel.Account;
            var friend = currentModel.Friend;
            var newSettings = currentModel.NewSettings;
            var oldSettings = currentModel.OldSettings;
            var isForSpy = currentModel.IsForSpy;

            if (!_accountManager.HasAWorkingAccount(account.Id))
            {
                return true;
            }

            if (account.GroupSettingsId == null)
            {
                return true;
            }

            if (account.GroupSettingsId == null)
            {
                return true;
            }

            var enabledGroupFunctions = _groupFunctionsService.GetEnabledFunctionsByGroupId((long)account.GroupSettingsId);
            var runningGroupFunctions = _jobStateService.GetStatesByAccountAndFunctionName(new JobStateViewModel
            {
                AccountId = account.Id,
                IsForSpy = isForSpy
            });

            foreach (var runningFunction in runningGroupFunctions)
            {
                if (enabledGroupFunctions.Any(
                        groupFunction => groupFunction.FunctionName == runningFunction.FunctionName) ||
                    _requaredFunctions.Any(groupFunction => groupFunction == runningFunction.FunctionName))
                {
                    continue;
                }

                RemoveBackgroundJobById(runningFunction.JobId);
            }

            foreach (var functionToRunModel in enabledGroupFunctions)
            {
                CreateBackgroundJob(new CreateBackgroundJobModel
                {
                    Account = account,
                    CheckPermissions = true,
                    IsForSpy = isForSpy,
                    FunctionName = functionToRunModel.FunctionName,
                    LaunchTime = SetLaunchTime(functionToRunModel.FunctionName, newSettings, oldSettings),
                    Friend = friend
                });
            }

            return true;
        }

        public bool CreateBackgroundJob(ICreateBackgroundJob model)
        {
            var currentModel = model as CreateBackgroundJobModel;

            if (currentModel == null)
            {
                return false;
            }

            var account = currentModel.Account;
            var isForSpy = currentModel.IsForSpy;
            var friend = currentModel.Friend;
            var checkPermissions = currentModel.CheckPermissions;
            var functionName = currentModel.FunctionName;
            var launchTime = currentModel.LaunchTime;

            var jobStateModel = new JobStateViewModel
            {
                AccountId = account.Id,
                FriendId = null,
                FunctionName = functionName,
                IsForSpy = isForSpy
            };

            if (_requaredFunctions.All(groupFunction => groupFunction != functionName) && checkPermissions)
            {
                if (!FunctionHasPermisions(functionName, account))
                {
                    return false;
                }
            }

            if (!TimeIsSet(launchTime))
            {
                return false;
            }

            if (JobIsRun(jobStateModel))
            {
                return false;
            }

            var runModel = new RunJobModel
            {
                Account = account,
                ForSpy = isForSpy,
                Friend = friend
            };
            var runJobFunctionService = new RunJobFunctionService();
            var jobId = BackgroundJob.Schedule(() => runJobFunctionService.RunService(functionName, runModel),
                launchTime);

            jobStateModel.JobId = jobId;

            AddJobState(jobStateModel);


            return true;
        }

        private static bool TimeIsSet(TimeSpan launchTime)
        {
            if (launchTime.Hours == 0 && launchTime.Minutes == 0 && launchTime.Seconds == 0)
            {
                return false;
            }
            
            return true;
        }

        private static bool AccountIsWorking(AccountViewModel account)
        {
            if (account.AuthorizationDataIsFailed || account.ProxyDataIsFailed || account.IsDeleted || account.ConformationDataIsFailed)
            {
                return false;
            }

            return true;
        }

        private static bool FunctionHasPermisions(FunctionName functionName, AccountViewModel account)
        {
            if (!new FunctionPermissionManager().HasPermissionsByFacebookId(functionName, account.FacebookId))
            {
                return false;
            }

            return true;
        }
       
        private bool JobIsRun(JobStateViewModel model)
        {
            return _jobStateService.CheckExist(model);
        }

        private void AddJobState(JobStateViewModel model)
        {
            _jobStateService.AddJobState(model);
        }

        private static TimeModel ConvertTimeSpanToTimeModel(TimeSpan timeSpan)
        {
            return new TimeModel
            {
                Hours = timeSpan.Hours,
                Minutes = timeSpan.Minutes,
                Seconds = timeSpan.Seconds
            };
        }

        private static TimeSpan SetLaunchTime(FunctionName functionName, GroupSettingsViewModel newSettings, GroupSettingsViewModel oldSettings)
        {
            switch (functionName)
            {
                case FunctionName.SendMessageToNewFriends:
                {
                    var newTime = new TimeSpan(newSettings.RetryTimeSendNewFriendHour,
                        newSettings.RetryTimeSendNewFriendMin, newSettings.RetryTimeSendNewFriendSec);

                    if (oldSettings == null)
                    {
                        return newTime;
                    }

                    var oldTime = new TimeSpan(oldSettings.RetryTimeSendNewFriendHour,
                        oldSettings.RetryTimeSendNewFriendMin, oldSettings.RetryTimeSendNewFriendSec);

                    if (SettingsAreEqual(newTime, oldTime))
                    {
                        return new TimeSpan(0, 0, 0);
                    }
                    return newTime;
                }

                case FunctionName.SendMessageToUnanswered:
                {
                    var newTime = new TimeSpan(newSettings.RetryTimeSendUnansweredHour,
                        newSettings.RetryTimeSendUnansweredMin, newSettings.RetryTimeSendUnansweredSec);

                    if (oldSettings == null)
                    {
                        return newTime;
                    }

                    var oldTime = new TimeSpan(oldSettings.RetryTimeSendUnansweredHour,
                        oldSettings.RetryTimeSendUnansweredMin, oldSettings.RetryTimeSendUnansweredSec);

                    if (SettingsAreEqual(newTime, oldTime))
                    {
                        return new TimeSpan(0, 0, 0);
                    }
                    return newTime;
                }
                case FunctionName.SendMessageToUnread:
                {
                    var newTime = new TimeSpan(newSettings.RetryTimeSendUnreadHour,
                        newSettings.RetryTimeSendUnreadMin, newSettings.RetryTimeSendUnreadSec);

                    if (oldSettings == null)
                    {
                        return newTime;
                    }

                    var oldTime = new TimeSpan(oldSettings.RetryTimeSendUnreadHour,
                        oldSettings.RetryTimeSendUnreadMin, oldSettings.RetryTimeSendUnreadSec);

                    if (SettingsAreEqual(newTime, oldTime))
                    {
                        return new TimeSpan(0, 0, 0);
                    }
                    return newTime;
                }
                case FunctionName.RefreshFriends:
                {
                    var newTime = new TimeSpan(newSettings.RetryTimeRefreshFriendsHour,
                        newSettings.RetryTimeRefreshFriendsMin, newSettings.RetryTimeRefreshFriendsSec);

                    if (oldSettings == null)
                    {
                        return newTime;
                    }

                    var oldTime = new TimeSpan(oldSettings.RetryTimeRefreshFriendsHour,
                        oldSettings.RetryTimeRefreshFriendsMin, oldSettings.RetryTimeRefreshFriendsSec);

                    if (SettingsAreEqual(newTime, oldTime))
                    {
                        return new TimeSpan(0, 0, 0);
                    }
                    return newTime;
                }
                case FunctionName.GetNewFriendsAndRecommended:
                {
                    var newTime = new TimeSpan(newSettings.RetryTimeGetNewAndRecommendedFriendsHour,
                        newSettings.RetryTimeGetNewAndRecommendedFriendsMin, newSettings.RetryTimeGetNewAndRecommendedFriendsSec);

                    if (oldSettings == null)
                    {
                        return newTime;
                    }

                    var oldTime = new TimeSpan(oldSettings.RetryTimeGetNewAndRecommendedFriendsHour,
                        oldSettings.RetryTimeGetNewAndRecommendedFriendsMin, oldSettings.RetryTimeGetNewAndRecommendedFriendsSec);

                    if (SettingsAreEqual(newTime, oldTime))
                    {
                        return new TimeSpan(0, 0, 0);
                    }
                    return newTime;
                }
                case FunctionName.ConfirmFriendship:
                {
                    var newTime = new TimeSpan(newSettings.RetryTimeConfirmFriendshipsHour,
                        newSettings.RetryTimeConfirmFriendshipsMin, newSettings.RetryTimeConfirmFriendshipsSec);

                    if (oldSettings == null)
                    {
                        return newTime;
                    }

                    var oldTime = new TimeSpan(oldSettings.RetryTimeConfirmFriendshipsHour,
                        oldSettings.RetryTimeConfirmFriendshipsMin, oldSettings.RetryTimeConfirmFriendshipsSec);

                    if (SettingsAreEqual(newTime, oldTime))
                    {
                        return new TimeSpan(0, 0, 0);
                    }
                    return newTime;
                }
                case FunctionName.SendRequestFriendship:
                {
                    var newTime = new TimeSpan(newSettings.RetryTimeSendRequestFriendshipsHour,
                        newSettings.RetryTimeSendRequestFriendshipsMin, newSettings.RetryTimeSendRequestFriendshipsSec);

                    if (oldSettings == null)
                    {
                        return newTime;
                    }

                    var oldTime = new TimeSpan(oldSettings.RetryTimeSendRequestFriendshipsHour,
                        oldSettings.RetryTimeSendRequestFriendshipsMin, oldSettings.RetryTimeSendRequestFriendshipsSec);

                    if (SettingsAreEqual(newTime, oldTime))
                    {
                        return new TimeSpan(0, 0, 0);
                    }
                    return newTime;
                }
                case FunctionName.InviteToGroups:
                {
                    var newTime = new TimeSpan(newSettings.RetryTimeInviteTheGroupsHour,
                        newSettings.RetryTimeInviteTheGroupsMin, newSettings.RetryTimeInviteTheGroupsSec);

                    if (oldSettings == null)
                    {
                        return newTime;
                    }

                    var oldTime = new TimeSpan(oldSettings.RetryTimeInviteTheGroupsHour,
                        oldSettings.RetryTimeInviteTheGroupsMin, oldSettings.RetryTimeInviteTheGroupsSec);

                    if (SettingsAreEqual(newTime, oldTime))
                    {
                        return new TimeSpan(0, 0, 0);
                    }
                    return newTime;
                }
                case FunctionName.InviteToPages:
                {
                    var newTime = new TimeSpan(newSettings.RetryTimeInviteThePagesHour,
                        newSettings.RetryTimeInviteThePagesMin, newSettings.RetryTimeInviteThePagesSec);

                    if (oldSettings == null)
                    {
                        return newTime;
                    }

                    var oldTime = new TimeSpan(oldSettings.RetryTimeInviteThePagesHour,
                        oldSettings.RetryTimeInviteThePagesMin, oldSettings.RetryTimeInviteThePagesSec);

                    if (SettingsAreEqual(newTime, oldTime))
                    {
                        return new TimeSpan(0, 0, 0);
                    }
                    return newTime;
                }
                case FunctionName.DialogIsOver:
                {
                    var newTime = new TimeSpan(newSettings.DialogIsOverTimer, 0, 0);

                    if (oldSettings == null)
                    {
                        //Первый запуск джоба, друг только что добавился
                        return newTime;
                    }

                    var oldTime = new TimeSpan(oldSettings.DialogIsOverTimer, 0, 0);

                    if (SettingsAreEqual(newTime, oldTime))
                    {
                        //Сохранили не измененные настройки
                        return new TimeSpan(0, 0, 0);
                    }

                    //Возвращаем разницу между текущим временем и временем добавления друга 
                    return newTime;
                }
                case FunctionName.IsAddedToGroupsAndPages:
                {
                    var newTime = new TimeSpan(newSettings.IsAddedToGroupsAndPagesTimer, 0, 0);

                    if (oldSettings == null)
                    {
                        return newTime;
                    }

                    var oldTime = new TimeSpan(oldSettings.IsAddedToGroupsAndPagesTimer, 0, 0);

                    if (SettingsAreEqual(newTime, oldTime))
                    {
                        return new TimeSpan(0, 0, 0);
                    }
                    return newTime;
                }
                case FunctionName.IsWink:
                {
                    var newTime = new TimeSpan(newSettings.IsWinkTimer, 0, 0);

                    if (oldSettings == null)
                    {
                        return newTime;
                    }

                    var oldTime = new TimeSpan(oldSettings.IsWinkTimer, 0, 0);

                    if (SettingsAreEqual(newTime, oldTime))
                    {
                        return new TimeSpan(0, 0, 0);
                    }
                    return newTime;
                }
                case FunctionName.IsWinkFriendsOfFriends:
                {
                    var newTime = new TimeSpan(newSettings.IsWinkFriendsOfFriendsTimer, 0, 0);

                    if (oldSettings == null)
                    {
                        return newTime;
                    }

                    var oldTime = new TimeSpan(oldSettings.IsWinkFriendsOfFriendsTimer, 0, 0);

                    if (SettingsAreEqual(newTime, oldTime))
                    {
                        return new TimeSpan(0, 0, 0);
                    }
                    return newTime;
                }
                case FunctionName.AnalyzeFriends:
                    break;
                case FunctionName.RefreshCookies:
                    break;
                case FunctionName.Wink:
                    {
                        var newTime = new TimeSpan(newSettings.RetryTimeForWinkFriendsHour, newSettings.RetryTimeForWinkFriendsMin, newSettings.RetryTimeForWinkFriendsSec);

                        if (oldSettings == null)
                        {
                            return newTime;
                        }

                        var oldTime = new TimeSpan(oldSettings.RetryTimeForWinkFriendsHour, oldSettings.RetryTimeForWinkFriendsMin, oldSettings.RetryTimeForWinkFriendsSec);

                        if (SettingsAreEqual(newTime, oldTime))
                        {
                            return new TimeSpan(0, 0, 0);
                        }
                        return newTime;
                    }
                case FunctionName.WinkFriendFriends:
                    {
                        var newTime = new TimeSpan(newSettings.RetryTimeForWinkFriendsFriendsHour, newSettings.RetryTimeForWinkFriendsFriendsMin, newSettings.RetryTimeForWinkFriendsFriendsSec);

                        if (oldSettings == null)
                        {
                            return newTime;
                        }

                        var oldTime = new TimeSpan(oldSettings.RetryTimeForWinkFriendsFriendsHour, oldSettings.RetryTimeForWinkFriendsFriendsMin, oldSettings.RetryTimeForWinkFriendsFriendsSec);

                        if (SettingsAreEqual(newTime, oldTime))
                        {
                            return new TimeSpan(0, 0, 0);
                        }
                        return newTime;
                    }
                case FunctionName.WinkBack:
                    {
                        var newTime = new TimeSpan(newSettings.RetryTimeForWinkBackHour, newSettings.RetryTimeForWinkBackMin, newSettings.RetryTimeForWinkBackSec);

                        if (oldSettings == null)
                        {
                            return newTime;
                        }

                        var oldTime = new TimeSpan(oldSettings.RetryTimeForWinkBackHour, oldSettings.RetryTimeForWinkBackMin, oldSettings.RetryTimeForWinkBackSec);

                        if (SettingsAreEqual(newTime, oldTime))
                        {
                            return new TimeSpan(0, 0, 0);
                        }
                        return newTime;
                    }
                default:
                    throw new ArgumentOutOfRangeException("functionName");
            }

            return new TimeSpan(0,0,0);
        }

        private static bool SettingsAreEqual(TimeSpan newTime, TimeSpan oldTime)
        {
            if (newTime.Hours == oldTime.Hours && newTime.Minutes == oldTime.Minutes && newTime.Seconds == oldTime.Seconds)
            {
                return true;
            }

            return false;
        }
    }
}
