using System;
using System.Collections.Generic;
using CommonInterfaces.Interfaces.Models;
using CommonInterfaces.Interfaces.Services;
using CommonModels;
using Constants.FunctionEnums;
using Hangfire;
using Jobs.Jobs.CommunityJobs;
using Jobs.Jobs.Cookies;
using Jobs.Jobs.DeleteFriendsJobs;
using Jobs.Jobs.FriendJobs;
using Jobs.Jobs.MessageJobs;
using Jobs.Jobs.WinksJobs;
using Jobs.Models;
using Services.Models.BackgroundJobs;
using Services.Models.Jobs;
using Services.Services;
using Services.ServiceTools;
using Services.ViewModels.GroupModels;
using Services.ViewModels.HomeModels;
using Services.ViewModels.JobStatusModels;
using AddOrUpdateAccountModel = Services.Models.BackgroundJobs.AddOrUpdateAccountModel;

namespace Jobs.JobsService
{
    public class BackgroundJobService : IBackgroundJobService
    {
        private readonly JobStatusService _jobStatusService;

        public BackgroundJobService()
        {
            _jobStatusService = new JobStatusService();
        }

        public void RemoveJobById(string jobId)
        {
            BackgroundJob.Delete(jobId);
        }

        public void RemoveJobsById(List<string> jobsId)
        {
            foreach (var jobId in jobsId)
            {
                BackgroundJob.Delete(jobId);
            }
        }

        public void RemoveAccountBackgroundJobs(IRemoveAccountJobs model)
        {
            var currentModel = model as RemoveAccountJobsModel;

            if (currentModel == null)
            {
                return;
            }

            var accountId = currentModel.AccountId;

            if (accountId == null)
            {
                return;
            }

            var jobStatusModel = new JobStatusViewModel
            {
                AccountId = (long) accountId,
                IsForSpy = currentModel.IsForSpy
            };

            var jobsId = _jobStatusService.DeleteJobStatusesByAccountId(jobStatusModel);

            RemoveJobsById(jobsId);
        }

        public bool AddOrUpdateAccountJobs(IAddOrUpdateAccountJobs model)
        {
            var currentModel = (AddOrUpdateAccountModel) model;

            if (currentModel == null)
            {
                return false;
            }

            var account = currentModel.Account;
            var newSettings = currentModel.NewSettings;
            var oldSettings = currentModel.OldSettings;
            var isForSpy = currentModel.IsForSpy;

            //cookies

            CreateBackgroundJob(new CreateBackgroundJobModel
            {
                Account = account,
                FunctionName = FunctionName.RefreshCookies,
                LaunchTime = new TimeSpan(2, 0, 0),
                CheckPermissions = false,
                IsForSpy = isForSpy
            });

            if (isForSpy)
            {
                return true;
            }

            if (!AccountIsWorking(account))
            {
                return false;
            }

            if (newSettings == null && oldSettings == null)
            {
                return false;
            }

            //community
            var joinTheGroupLaunchTime = new TimeSpan(0, 0, 10);
            var inviteToGroupLaunchTime = SetLaunchTime(FunctionName.InviteToGroups, newSettings, oldSettings);
            var inviteToPageLaunchTime = SetLaunchTime(FunctionName.InviteToPages, newSettings, oldSettings);


            CreateBackgroundJob(new CreateBackgroundJobModel
            {
                Account = account,
                FunctionName = FunctionName.JoinTheNewGroupsAndPages,
                LaunchTime = joinTheGroupLaunchTime,
                CheckPermissions = true
            });

            CreateBackgroundJob(new CreateBackgroundJobModel
            {
                Account = account,
                FunctionName = FunctionName.JoinTheNewGroupsAndPages,
                LaunchTime = joinTheGroupLaunchTime,
                CheckPermissions = true
            });

            CreateBackgroundJob(new CreateBackgroundJobModel
            {
                Account = account,
                FunctionName = FunctionName.InviteToGroups,
                LaunchTime = inviteToGroupLaunchTime,
                CheckPermissions = true
            });

            CreateBackgroundJob(new CreateBackgroundJobModel
            {
                Account = account,
                FunctionName = FunctionName.InviteToPages,
                LaunchTime = inviteToPageLaunchTime,
                CheckPermissions = true
            });

            //friends
            var refreshFriendsLaunchTime = SetLaunchTime(FunctionName.RefreshFriends, newSettings, oldSettings);
            var confirmFriendshipsLaunchTime = SetLaunchTime(FunctionName.ConfirmFriendship, newSettings, oldSettings);
            var getNewAndRecommendedFriendsLaunchTime = SetLaunchTime(FunctionName.GetNewFriendsAndRecommended,
                newSettings, oldSettings);
            var sendRequestFriendshipsLaunchTime = SetLaunchTime(FunctionName.SendRequestFriendship, newSettings,
                oldSettings);

            CreateBackgroundJob(new CreateBackgroundJobModel
            {
                Account = account,
                FunctionName = FunctionName.RefreshFriends,
                LaunchTime = refreshFriendsLaunchTime,
                CheckPermissions = true
            });

            CreateBackgroundJob(new CreateBackgroundJobModel
            {
                Account = account,
                FunctionName = FunctionName.ConfirmFriendship,
                LaunchTime = confirmFriendshipsLaunchTime,
                CheckPermissions = true
            });

            CreateBackgroundJob(new CreateBackgroundJobModel
            {
                Account = account,
                FunctionName = FunctionName.GetNewFriendsAndRecommended,
                LaunchTime = getNewAndRecommendedFriendsLaunchTime,
                CheckPermissions = true
            });

            CreateBackgroundJob(new CreateBackgroundJobModel
            {
                Account = account,
                FunctionName = FunctionName.SendRequestFriendship,
                LaunchTime = sendRequestFriendshipsLaunchTime,
                CheckPermissions = true
            });


            //winks
            var winkFriendshipsLaunchTime = SetLaunchTime(FunctionName.Wink, newSettings, oldSettings);
            var winkFriendsFriendshipsLaunchTime = SetLaunchTime(FunctionName.WinkFriendFriends, newSettings, oldSettings);
            var winkBackLaunchTime = SetLaunchTime(FunctionName.WinkBack, newSettings, oldSettings);

            CreateBackgroundJob(new CreateBackgroundJobModel
            {
                Account = account,
                FunctionName = FunctionName.Wink,
                LaunchTime = winkFriendshipsLaunchTime,
                CheckPermissions = true
            });

            CreateBackgroundJob(new CreateBackgroundJobModel
            {
                Account = account,
                FunctionName = FunctionName.WinkFriendFriends,
                LaunchTime = winkFriendsFriendshipsLaunchTime,
                CheckPermissions = true
            });

            CreateBackgroundJob(new CreateBackgroundJobModel
            {
                Account = account,
                FunctionName = FunctionName.WinkBack,
                LaunchTime = winkBackLaunchTime,
                CheckPermissions = true
            });
            
            //messages
            var sendUnreadLaunchTime = SetLaunchTime(FunctionName.SendMessageToUnread, newSettings, oldSettings);
            var sendUnansweredLaunchTime = SetLaunchTime(FunctionName.SendMessageToUnanswered, newSettings, oldSettings);
            var sendNewFriendLaunchTime = SetLaunchTime(FunctionName.SendMessageToNewFriends, newSettings, oldSettings);

            CreateBackgroundJob(new CreateBackgroundJobModel
            {
                Account = account,
                FunctionName = FunctionName.SendMessageToUnread,
                LaunchTime = sendUnreadLaunchTime,
                CheckPermissions = true
            });

            CreateBackgroundJob(new CreateBackgroundJobModel
            {
                Account = account,
                FunctionName = FunctionName.SendMessageToUnanswered,
                LaunchTime = sendUnansweredLaunchTime,
                CheckPermissions = true
            });

            CreateBackgroundJob(new CreateBackgroundJobModel
            {
                Account = account,
                FunctionName = FunctionName.SendMessageToNewFriends,
                LaunchTime = sendNewFriendLaunchTime,
                CheckPermissions = true
            });
            
            return true;
        }

        public void CreateBackgroundJob(ICreateBackgroundJob model)
        {
            var currentModel = model as CreateBackgroundJobModel;

            if (currentModel == null)
            {
                return;
            }

            var account = currentModel.Account;
            var isForSpy = currentModel.IsForSpy;
            var friend = currentModel.Friend;
            var checkPermissions = currentModel.CheckPermissions;
            var functionName = currentModel.FunctionName;
            var launchTime = currentModel.LaunchTime;
            var launchTimeModel = ConvertTimeSpanToTimeModel(launchTime);

            if (checkPermissions)
            {
                if (!FunctionHasPermisions(functionName, account))
                {
                    return;
                }
            }
            if (!TimeIsSet(launchTime))
            {
                return;
            }

            var jobStatusModel = new JobStatusViewModel
            {
                AccountId = account.Id,
                LaunchTime = launchTimeModel,
                FriendId = null,
                FunctionName = functionName,
                IsForSpy = isForSpy
            };

            if (JobIsRun(jobStatusModel))
            {
                var jobStatusList = new JobStatusService().GetJobStatus(jobStatusModel);

                if (jobStatusList != null)
                {
                    foreach (var jobStatusViewModel in jobStatusList)
                    {
                        try
                        {
                            BackgroundJob.Delete(jobStatusViewModel.JobId);
                            if (true)
                            {
                                new JobStatusService().DeleteJobStatus(jobStatusModel);
                            }
                        }
                        catch (Exception)
                        {
                            
                        }
                    }

                }
            }

            var runModel = new RunJobModel
            {
                Account = account,
                ForSpy = isForSpy,
                Friend = friend
            };

            switch (functionName)
            {
                case FunctionName.SendMessageToNewFriends:
                {
                    var jobId = BackgroundJob.Schedule(() => SendMessageToNewFriendsJob.Run(runModel), launchTime);
                    //Помечаем запуск джоба
                    jobStatusModel.JobId = jobId;

                    AddJobStatus(jobStatusModel);
                    break;
                }
                case FunctionName.SendMessageToUnanswered:
                {
                    var jobId = BackgroundJob.Schedule(() => SendMessageToUnansweredJob.Run(runModel), launchTime);
                    //Помечаем запуск джоба
                    jobStatusModel.JobId = jobId;

                    AddJobStatus(jobStatusModel);
                    break;
                }
                case FunctionName.SendMessageToUnread:
                {
                    var jobId = BackgroundJob.Schedule(() => SendMessageToUnreadJob.Run(runModel), launchTime);
                    //Помечаем запуск джоба
                    jobStatusModel.JobId = jobId;

                    AddJobStatus(jobStatusModel);
                    break;
                }
                case FunctionName.RefreshFriends:
                {
                    var jobId = BackgroundJob.Schedule(() => RefreshFriendsJob.Run(runModel), launchTime);
                    //Помечаем запуск джоба
                    jobStatusModel.JobId = jobId;

                    AddJobStatus(jobStatusModel);
                    break;
                }
                case FunctionName.GetNewFriendsAndRecommended:
                {
                    var jobId = BackgroundJob.Schedule(() => GetNewFriendsAndRecommendedJob.Run(runModel), launchTime);
                    //Помечаем запуск джоба
                    jobStatusModel.JobId = jobId;

                    AddJobStatus(jobStatusModel);
                    break;
                }
                case FunctionName.ConfirmFriendship:
                {
                    var jobId = BackgroundJob.Schedule(() => ConfirmFriendshipJob.Run(runModel), launchTime);
                    //Помечаем запуск джоба
                    jobStatusModel.JobId = jobId;

                    AddJobStatus(jobStatusModel);
                    break;
                }
                case FunctionName.SendRequestFriendship:
                {
                    var jobId = BackgroundJob.Schedule(() => SendRequestFriendshipJob.Run(runModel), launchTime);
                    //Помечаем запуск джоба
                    jobStatusModel.JobId = jobId;

                    AddJobStatus(jobStatusModel);
                    break;
                }
                case FunctionName.AnalyzeFriends:
                {
                    var jobId = BackgroundJob.Schedule(() => SendRequestFriendshipJob.Run(runModel), launchTime);
                    //Помечаем запуск джоба
                    jobStatusModel.JobId = jobId;

                    AddJobStatus(jobStatusModel);
                    break;
                }
                case FunctionName.RefreshCookies:
                {
                    var jobId = BackgroundJob.Schedule(() => RefreshCookiesJob.Run(runModel), launchTime);
                    //Помечаем запуск джоба
                    jobStatusModel.JobId = jobId;

                    AddJobStatus(jobStatusModel);
                    break;
                }
                case FunctionName.JoinTheNewGroupsAndPages:
                {
                    if (account.GroupSettingsId == null)
                    {
                        break;
                    }
                    var newGroups = new GroupService(new NoticeService()).GetNewSettings(account.Id, (long) account.GroupSettingsId);

                    if (newGroups == null || newGroups.Count == 0)
                    {
                        break;
                    }

                    var jobId = BackgroundJob.Schedule(() => JoinTheNewGroupsAndPagesJob.Run(runModel), launchTime);
                    //Помечаем запуск джоба
                    jobStatusModel.JobId = jobId;

                    AddJobStatus(jobStatusModel);
                    break;
                }
                case FunctionName.InviteToGroups:
                {
                    var jobId = BackgroundJob.Schedule(() => InviteTheNewGroupJob.Run(runModel), launchTime);
                    //Помечаем запуск джоба
                    jobStatusModel.JobId = jobId;

                    AddJobStatus(jobStatusModel);
                    break;
                }
                case FunctionName.InviteToPages:
                {
                    var jobId = BackgroundJob.Schedule(() => InviteTheNewPageJob.Run(runModel), launchTime);
                    //Помечаем запуск джоба
                    jobStatusModel.JobId = jobId;

                    AddJobStatus(jobStatusModel);
                    break;
                }
                case FunctionName.RemoveFromFriends:
                {
                    var jobId = BackgroundJob.Schedule(() => RemoveFromFriendsJob.Run(runModel), launchTime);
                    //Помечаем запуск джоба
                    jobStatusModel.JobId = jobId;

                    AddJobStatus(jobStatusModel);
                    break;
                }
                case FunctionName.Wink:
                {
                    var jobId = BackgroundJob.Schedule(() => WinkFriendsJob.Run(runModel), launchTime);
                    //Помечаем запуск джоба
                    jobStatusModel.JobId = jobId;

                    AddJobStatus(jobStatusModel);
                    break;
                }
                case FunctionName.WinkFriendFriends:
                {
                    var jobId = BackgroundJob.Schedule(() => WinkFriendsFriendsJob.Run(runModel), launchTime);
                    //Помечаем запуск джоба
                    jobStatusModel.JobId = jobId;

                    AddJobStatus(jobStatusModel);
                    break;
                }
                case FunctionName.WinkBack:
                {
                    var jobId = BackgroundJob.Schedule(() => WinkBackJob.Run(runModel), launchTime);
                    //Помечаем запуск джоба
                    jobStatusModel.JobId = jobId;

                    AddJobStatus(jobStatusModel);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException("functionName");
            }
        }

        private void AddJobStatus(JobStatusViewModel model)
        {
            new JobStatusService().AddJobStatus(model);
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

        private static bool JobIsRun(JobStatusViewModel model)
        {
            if (new JobStatusService().JobIsInRun(model))
            {
                return true;
            }

            return false;
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
