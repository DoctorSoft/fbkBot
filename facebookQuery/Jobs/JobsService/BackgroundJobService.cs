using System;
using Constants.FunctionEnums;
using Hangfire;
using Jobs.Jobs.Cookies;
using Jobs.Jobs.FriendJobs;
using Jobs.Jobs.MessageJobs;
using Jobs.Jobs.SettingsJobs;
using Services.Hubs;
using Services.Interfaces;
using Services.Services;
using Services.ServiceTools;
using Services.ViewModels.GroupModels;
using Services.ViewModels.HomeModels;

namespace Jobs.JobsService
{
    public class BackgroundJobService : IBackgroundJobService
    {
        private readonly NotificationHub _notice;

        public BackgroundJobService()
        {
            _notice = new NotificationHub();
        }
        public bool AddOrUpdateAccountJobs(AccountViewModel account, GroupSettingsViewModel newSettings, GroupSettingsViewModel oldSettings)
        {
            _notice.Add(account.Id, "Обновляем обработчик событий");
            //cookies
            CreateBackgroundJob(account, FunctionName.RefreshCookies, new TimeSpan(2, 0, 0), true);

            if (!AccountIsWorking(account))
            {
                return false;
            }
            
            //community
            var joinTheGroupLaunchTime = new TimeSpan(0, 0, 1);
            CreateBackgroundJob(account, FunctionName.JoinTheNewGroup, joinTheGroupLaunchTime, false);

            //friends
            var refreshFriendsLaunchTime = SetLaunchTime(FunctionName.RefreshFriends, newSettings, oldSettings);
            var confirmFriendshipsLaunchTime = SetLaunchTime(FunctionName.ConfirmFriendship, newSettings, oldSettings);
            var getNewAndRecommendedFriendsLaunchTime = SetLaunchTime(FunctionName.GetNewFriendsAndRecommended, newSettings, oldSettings);
            var sendRequestFriendshipsLaunchTime = SetLaunchTime(FunctionName.SendRequestFriendship, newSettings, oldSettings);

            CreateBackgroundJob(account, FunctionName.RefreshFriends, refreshFriendsLaunchTime, true);
            CreateBackgroundJob(account, FunctionName.ConfirmFriendship, confirmFriendshipsLaunchTime, true);
            CreateBackgroundJob(account, FunctionName.GetNewFriendsAndRecommended, getNewAndRecommendedFriendsLaunchTime, true);
            CreateBackgroundJob(account, FunctionName.SendRequestFriendship, sendRequestFriendshipsLaunchTime, true);

            //messages
            var sendUnreadLaunchTime = SetLaunchTime(FunctionName.SendMessageToUnread, newSettings, oldSettings);
            var sendUnansweredLaunchTime = SetLaunchTime(FunctionName.SendMessageToUnanswered, newSettings, oldSettings);
            var sendNewFriendLaunchTime = SetLaunchTime(FunctionName.SendMessageToNewFriends, newSettings, oldSettings);

            CreateBackgroundJob(account, FunctionName.SendMessageToUnread, sendUnreadLaunchTime, true);
            CreateBackgroundJob(account, FunctionName.SendMessageToUnanswered, sendUnansweredLaunchTime, true);
            CreateBackgroundJob(account, FunctionName.SendMessageToNewFriends, sendNewFriendLaunchTime, true);

            return true;
        }

        public void CreateBackgroundJob(AccountViewModel account, FunctionName functionName, TimeSpan launchTime, bool checkPermissions)
        {
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

            switch (functionName)
            {
                case FunctionName.SendMessageToNewFriends:
                {
                    BackgroundJob.Schedule(() => SendMessageToNewFriendsJob.Run(account), launchTime);
                    break;
                }
                case FunctionName.SendMessageToUnanswered:
                {
                    BackgroundJob.Schedule(() => SendMessageToUnansweredJob.Run(account), launchTime);
                    break;
                }
                case FunctionName.SendMessageToUnread:
                {
                    BackgroundJob.Schedule(() => SendMessageToUnreadJob.Run(account), launchTime);
                    break;
                }
                case FunctionName.RefreshFriends:
                {
                    BackgroundJob.Schedule(() => RefreshFriendsJob.Run(account), launchTime);
                    break;
                }
                case FunctionName.GetNewFriendsAndRecommended:
                {
                    BackgroundJob.Schedule(() => GetNewFriendsAndRecommendedJob.Run(account), launchTime);
                    break;
                }
                case FunctionName.ConfirmFriendship:
                {
                    BackgroundJob.Schedule(() => ConfirmFriendshipJob.Run(account), launchTime);
                    break;
                }
                case FunctionName.SendRequestFriendship:
                {
                    BackgroundJob.Schedule(() => SendRequestFriendshipJob.Run(account), launchTime);
                    break;
                }
                case FunctionName.AnalyzeFriends:
                {
                    BackgroundJob.Schedule(() => SendRequestFriendshipJob.Run(account), launchTime);
                    break;
                }
                case FunctionName.RefreshCookies:
                {
                    BackgroundJob.Schedule(() => RefreshCookiesJob.Run(account), launchTime);
                    break;
                }
                case FunctionName.JoinTheNewGroup:
                {
                    if (account.GroupSettingsId == null)
                    {
                        break;
                    }
                    var newGroups = new GroupService().GetNewSettings(account.Id, (long) account.GroupSettingsId);

                    if (newGroups == null || newGroups.Count == 0)
                    {
                        break;
                    }

                    BackgroundJob.Schedule(() => JoinTheNewGroupJob.Run(account), launchTime);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException("functionName");
            }
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
