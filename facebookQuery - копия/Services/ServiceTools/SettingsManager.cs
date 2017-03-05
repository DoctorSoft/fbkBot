using System;
using CommonModels;
using Constants.FunctionEnums;
using DataBase.Context;
using DataBase.QueriesAndCommands.Queries.JobStatus;
using DataBase.QueriesAndCommands.Queries.Settings;
using Services.Interfaces.ServiceTools;
using Services.ViewModels.HomeModels;

namespace Services.ServiceTools
{
    public class SettingsManager : ISettingsManager
    {
        public bool HasARetryTimePermission(FunctionName functionName, AccountViewModel account)
        {
            if (account.GroupSettingsId == null)
            {
                return false;
            }

            var delay = GetDelay(functionName, (long) account.GroupSettingsId);

            var jobStatus = new GetJobStatusQueryHandler(new DataBaseContext()).Handle(new GetJobStatusQuery
            {
                FunctionName = functionName,
                AccountId = account.Id
            });

            if (jobStatus == null)
            {
                return true;
            }

            return CheckDelay(jobStatus.LastLaunchDateTime, delay);
        }

        private static bool CheckDelay(DateTime lastLaunchDateTime, TimeModel delay)
        {
            var differenceTime = DateTime.Now - lastLaunchDateTime;
            var summ = differenceTime.Days * 24 * 60 + differenceTime.Hours * 60 + differenceTime.Minutes;
            //return summ >= delay;

            return false;
        }

        public TimeModel GetDelay(FunctionName functionName, long groupSettingsId)
        {
            var groupSettings =
            new GetSettingsByGroupSettingsIdQueryHandler(new DataBaseContext()).Handle(
                new GetSettingsByGroupSettingsIdQuery
                {
                    GroupSettingsId = groupSettingsId
                });

            switch (functionName)
            {
                case FunctionName.SendMessageToNewFriends:
                {
                    return groupSettings.MessageOptions.RetryTimeSendNewFriend;
                }
                case FunctionName.SendMessageToUnanswered:
                {
                    return groupSettings.MessageOptions.RetryTimeSendUnanswered;
                }
                case FunctionName.SendMessageToUnread:
                {
                    return groupSettings.MessageOptions.RetryTimeSendUnread;
                }
                case FunctionName.RefreshFriends:
                {
                    return groupSettings.FriendsOptions.RetryTimeRefreshFriends;
                }
                case FunctionName.GetNewFriendsAndRecommended:
                {
                    return groupSettings.FriendsOptions.RetryTimeGetNewAndRecommendedFriends;
                }
                case FunctionName.ConfirmFriendship:
                {
                    return groupSettings.FriendsOptions.RetryTimeConfirmFriendships;
                }
                case FunctionName.SendRequestFriendship:
                {
                    return groupSettings.FriendsOptions.RetryTimeSendRequestFriendships;
                }
                default:
                    throw new ArgumentOutOfRangeException("functionName");
            }
        }

        public TimeSpan GetTimeSpanByFunctionName(FunctionName functionName, long groupSettingsId)
        {
            var groupSettings =
                new GetSettingsByGroupSettingsIdQueryHandler(new DataBaseContext()).Handle(
                    new GetSettingsByGroupSettingsIdQuery
                    {
                        GroupSettingsId = groupSettingsId
                    });

            switch (functionName)
            {
                case FunctionName.SendMessageToNewFriends:
                {
                    var sendMessageToNewFriendsTimeModel = groupSettings.MessageOptions.RetryTimeSendNewFriend;
                    return new TimeSpan(sendMessageToNewFriendsTimeModel.Hours, sendMessageToNewFriendsTimeModel.Minutes,
                        sendMessageToNewFriendsTimeModel.Seconds);
                }
                case FunctionName.SendMessageToUnanswered:
                {
                    var sendMessageToUnansweredTimeModel = groupSettings.MessageOptions.RetryTimeSendUnanswered;
                    return new TimeSpan(sendMessageToUnansweredTimeModel.Hours, sendMessageToUnansweredTimeModel.Minutes,
                        sendMessageToUnansweredTimeModel.Seconds);
                }
                case FunctionName.SendMessageToUnread:
                {
                    var sendMessageToUnreadTimeModel = groupSettings.MessageOptions.RetryTimeSendUnread;
                    return new TimeSpan(sendMessageToUnreadTimeModel.Hours, sendMessageToUnreadTimeModel.Minutes,
                        sendMessageToUnreadTimeModel.Seconds);
                }
                case FunctionName.RefreshFriends:
                {
                    var refreshFriendsTimeModel = groupSettings.FriendsOptions.RetryTimeRefreshFriends;
                    return new TimeSpan(refreshFriendsTimeModel.Hours, refreshFriendsTimeModel.Minutes,
                        refreshFriendsTimeModel.Seconds);
                }
                case FunctionName.GetNewFriendsAndRecommended:
                {
                    var getNewFriendsAndRecommendedTimeModel = groupSettings.FriendsOptions.RetryTimeGetNewAndRecommendedFriends;
                    return new TimeSpan(getNewFriendsAndRecommendedTimeModel.Hours,
                        getNewFriendsAndRecommendedTimeModel.Minutes,
                        getNewFriendsAndRecommendedTimeModel.Seconds);
                }
                case FunctionName.ConfirmFriendship:
                {
                    var confirmFriendshipTimeModel = groupSettings.FriendsOptions.RetryTimeConfirmFriendships;
                    return new TimeSpan(confirmFriendshipTimeModel.Hours, confirmFriendshipTimeModel.Minutes,
                        confirmFriendshipTimeModel.Seconds);
                }
                case FunctionName.SendRequestFriendship:
                {
                    var sendRequestFriendshipTimeModel = groupSettings.FriendsOptions.RetryTimeSendRequestFriendships;
                    return new TimeSpan(sendRequestFriendshipTimeModel.Hours, sendRequestFriendshipTimeModel.Minutes,
                        sendRequestFriendshipTimeModel.Seconds);
                }
                default:
                    throw new ArgumentOutOfRangeException("functionName");
            }
        }
    }
}
