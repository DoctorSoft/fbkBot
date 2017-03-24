using System;
using CommonInterfaces.Interfaces.Models;
using CommonInterfaces.Interfaces.Services;
using Constants.FunctionEnums;
using Hangfire;
using Jobs.Jobs.CommunityJobs;
using Jobs.Jobs.Cookies;
using Jobs.Jobs.DeleteFriendsJobs;
using Jobs.Jobs.FriendJobs;
using Jobs.Jobs.MessageJobs;
using Jobs.Notices;
using Services.Hubs;
using Services.Models.BackgroundJobs;
using Services.Services;
using Services.ServiceTools;
using Services.ViewModels.GroupModels;
using Services.ViewModels.HomeModels;
using Services.ViewModels.JobStatusModels;

namespace Jobs.JobsService
{
    public class BackgroundJobService : IBackgroundJobService
    {
        private readonly NotificationHub _notice;

        public BackgroundJobService()
        {
            _notice = new NotificationHub();
        }

        public bool AddOrUpdateDeleteFriendsJobs(IAddOrUpdateAccountJobs model)
        {
            var currentModel = model as AddOrUpdateAccountModel;

            if (currentModel == null)
            {
                return false;
            }

            var account = currentModel.Account;
            var newSettings = currentModel.NewSettings;
            var oldSettings = currentModel.OldSettings;
            var friend = currentModel.Friend;
            
            _notice.Add(account.Id, "Обновляем время до постановки друзей в очередь удаления.");

            //delete friends
            var dialogIsOverLaunchTime = SetLaunchTime(FunctionName.DialogIsOver, newSettings, oldSettings);
            var isAddedToGroupsAndPagesLaunchTime = SetLaunchTime(FunctionName.IsAddedToGroupsAndPages, newSettings,
                oldSettings);
            var isWinkLaunchTime = SetLaunchTime(FunctionName.IsWink, newSettings, oldSettings);
            var isWinkFriendsOfFriendsLaunchTime = SetLaunchTime(FunctionName.IsWinkFriendsOfFriends, newSettings,
                oldSettings);

            CreateBackgroundJobForDeleteFriends(new CreateBackgroundJobForDeleteFriendsModel
            {
                Account = account,
                FunctionName = FunctionName.DialogIsOver,
                LaunchHourTime = dialogIsOverLaunchTime.Hours,
                Friend = friend
            });

            CreateBackgroundJobForDeleteFriends(new CreateBackgroundJobForDeleteFriendsModel
            {
                Account = account,
                FunctionName = FunctionName.IsAddedToGroupsAndPages,
                LaunchHourTime = isAddedToGroupsAndPagesLaunchTime.Hours,
                Friend = friend
            });

            CreateBackgroundJobForDeleteFriends(new CreateBackgroundJobForDeleteFriendsModel
            {
                Account = account,
                FunctionName = FunctionName.IsWink,
                LaunchHourTime = isWinkLaunchTime.Hours,
                Friend = friend
            });

            CreateBackgroundJobForDeleteFriends(new CreateBackgroundJobForDeleteFriendsModel
            {
                Account = account,
                FunctionName = FunctionName.IsWinkFriendsOfFriends,
                LaunchHourTime = isWinkFriendsOfFriendsLaunchTime.Hours,
                Friend = friend
            });

            return true;
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

            _notice.Add(account.Id, "Обновляем обработчик событий");
            //cookies

            CreateBackgroundJob(new Services.Models.BackgroundJobs.CreateBackgroundJobModel
            {
                Account = account,
                FunctionName = FunctionName.RefreshCookies,
                LaunchTime = new TimeSpan(2, 0, 0),
                CheckPermissions = true
            });

            if (!AccountIsWorking(account))
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

        public void CreateBackgroundJobForDeleteFriends(ICreateBackgroundJobForDeleteFriends model)
        {
            var currentModel = model as CreateBackgroundJobForDeleteFriendsModel;

            if (currentModel == null)
            {
                return;
            }

            var account = currentModel.Account;
            var friend = currentModel.Friend;
            var functionName = currentModel.FunctionName;
            var launchTimeHours = currentModel.LaunchHourTime - (DateTime.Now - friend.AddDateTime).Hours;
            var launchTime = launchTimeHours < 0 ? new TimeSpan(0,0,1,0) : new TimeSpan(0, launchTimeHours, 0, 0);

            if (JobIsRun(functionName, account))
            {
                // Если джоб создан
                var jobStatus = new JobStatusService().GetJobStatus(account.Id, functionName, friend.Id);
                
                if (jobStatus != null)
                {
                    foreach (var jobStatusViewModel in jobStatus)
                    {
                        BackgroundJob.Delete(jobStatusViewModel.JobId);
                    }
                }
            
                new JobStatusService().DeleteJobStatus(account.Id, functionName, friend.Id);
            }

            var jobStatusModel = new JobStatusViewModel
            {
                AccountId = account.Id,
                LaunchDateTime = launchTime,
                FriendId = friend.Id,
                FunctionName = functionName
            };

            switch (functionName)
            {
                case FunctionName.DialogIsOver:
                {
                    var jobId = BackgroundJob.Schedule(() => DialogIsOverConditionJob.Run(account, friend), launchTime);
                    jobStatusModel.JobId = jobId;

                    AddJobStatus(jobStatusModel);
                    break;
                }
                case FunctionName.IsAddedToGroupsAndPages:
                {
                    var jobId = BackgroundJob.Schedule(() => IsAddedToGroupsAndPagesConditionJob.Run(account, friend), launchTime);
                    jobStatusModel.JobId = jobId;

                    AddJobStatus(jobStatusModel);
                    break;
                }
                case FunctionName.IsWink:
                {
                    var jobId = BackgroundJob.Schedule(() => IsWinkConditionJob.Run(account, friend), launchTime);
                    jobStatusModel.JobId = jobId;

                    AddJobStatus(jobStatusModel);
                    break;
                }
                case FunctionName.IsWinkFriendsOfFriends:
                {
                    var jobId = BackgroundJob.Schedule(() => IsWinkFriendsOfFriendsConditionJob.Run(account, friend), launchTime);
                    jobStatusModel.JobId = jobId;

                    AddJobStatus(jobStatusModel);
                    break;
                }
            }
        }
        
        public void CreateBackgroundJob(ICreateBackgroundJob model)
        {
            var currentModel = model as CreateBackgroundJobModel;

            if (currentModel == null)
            {
                return;
            }

            var account = currentModel.Account;
            var checkPermissions = currentModel.CheckPermissions;
            var functionName = currentModel.FunctionName;
            var launchTime = currentModel.LaunchTime;

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

            if (JobIsRun(functionName, account))
            {
                var jobStatusList = new JobStatusService().GetJobStatus(account.Id, functionName, null);

                foreach (var jobStatusViewModel in jobStatusList)
                {
                    BackgroundJob.Delete(jobStatusViewModel.JobId);
                }

                new JobStatusService().DeleteJobStatus(account.Id, functionName, null);
            }

            var jobStatusModel = new JobStatusViewModel
            {
                AccountId = account.Id,
                LaunchDateTime = launchTime,
                FriendId = null,
                FunctionName = functionName
            };

            switch (functionName)
            {
                case FunctionName.SendMessageToNewFriends:
                {
                    var jobId = BackgroundJob.Schedule(() => SendMessageToNewFriendsJob.Run(account), launchTime);
                    //Помечаем запуск джоба
                    jobStatusModel.JobId = jobId;

                    AddJobStatus(jobStatusModel);
                    break;
                }
                case FunctionName.SendMessageToUnanswered:
                {
                    var jobId = BackgroundJob.Schedule(() => SendMessageToUnansweredJob.Run(account), launchTime);
                    //Помечаем запуск джоба
                    jobStatusModel.JobId = jobId;

                    AddJobStatus(jobStatusModel);
                    break;
                }
                case FunctionName.SendMessageToUnread:
                {
                    var jobId = BackgroundJob.Schedule(() => SendMessageToUnreadJob.Run(account), launchTime);
                    //Помечаем запуск джоба
                    jobStatusModel.JobId = jobId;

                    AddJobStatus(jobStatusModel);
                    break;
                }
                case FunctionName.RefreshFriends:
                {
                    var jobId = BackgroundJob.Schedule(() => RefreshFriendsJob.Run(account), launchTime);
                    //Помечаем запуск джоба
                    jobStatusModel.JobId = jobId;

                    AddJobStatus(jobStatusModel);
                    break;
                }
                case FunctionName.GetNewFriendsAndRecommended:
                {
                    var jobId = BackgroundJob.Schedule(() => GetNewFriendsAndRecommendedJob.Run(account), launchTime);
                    //Помечаем запуск джоба
                    jobStatusModel.JobId = jobId;

                    AddJobStatus(jobStatusModel);
                    break;
                }
                case FunctionName.ConfirmFriendship:
                {
                    var jobId = BackgroundJob.Schedule(() => ConfirmFriendshipJob.Run(account), launchTime);
                    //Помечаем запуск джоба
                    jobStatusModel.JobId = jobId;

                    AddJobStatus(jobStatusModel);
                    break;
                }
                case FunctionName.SendRequestFriendship:
                {
                    var jobId = BackgroundJob.Schedule(() => SendRequestFriendshipJob.Run(account), launchTime);
                    //Помечаем запуск джоба
                    jobStatusModel.JobId = jobId;

                    AddJobStatus(jobStatusModel);
                    break;
                }
                case FunctionName.AnalyzeFriends:
                {
                    var jobId = BackgroundJob.Schedule(() => SendRequestFriendshipJob.Run(account), launchTime);
                    //Помечаем запуск джоба
                    jobStatusModel.JobId = jobId;

                    AddJobStatus(jobStatusModel);
                    break;
                }
                case FunctionName.RefreshCookies:
                {
                    var jobId = BackgroundJob.Schedule(() => RefreshCookiesJob.Run(account), launchTime);
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
                    var newGroups = new GroupService(new NoticesProxy()).GetNewSettings(account.Id, (long) account.GroupSettingsId);

                    if (newGroups == null || newGroups.Count == 0)
                    {
                        break;
                    }

                    var jobId = BackgroundJob.Schedule(() => JoinTheNewGroupsAndPagesJob.Run(account), launchTime);
                    //Помечаем запуск джоба
                    jobStatusModel.JobId = jobId;

                    AddJobStatus(jobStatusModel);
                    break;
                }
                case FunctionName.InviteToGroups:
                {
                    var jobId = BackgroundJob.Schedule(() => InviteTheNewGroupJob.Run(account), launchTime);
                    //Помечаем запуск джоба
                    jobStatusModel.JobId = jobId;

                    AddJobStatus(jobStatusModel);
                    break;
                }
                case FunctionName.InviteToPages:
                {
                    var jobId = BackgroundJob.Schedule(() => InviteTheNewPageJob.Run(account), launchTime);
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
            if (account.AuthorizationDataIsFailed || account.ProxyDataIsFailed || account.IsDeleted)
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
        private static bool JobIsRun(FunctionName functionName, AccountViewModel account)
        {
            if (new JobStatusService().JobIsInRun(account.Id, functionName))
            {
                return true;
            }

            return false;
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
