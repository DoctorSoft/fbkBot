using System;
using Constants.FunctionEnums;
using DataBase.Context;
using DataBase.QueriesAndCommands.Queries.JobStatus;
using DataBase.QueriesAndCommands.Queries.Settings;
using Services.Core.Interfaces.ServiceTools;
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

        private static bool CheckDelay(DateTime lastLaunchDateTime, long delay)
        {
            var differenceTime = DateTime.Now - lastLaunchDateTime;
            var summ = differenceTime.Days * 24 * 60 + differenceTime.Hours * 60 + differenceTime.Minutes;
            return summ >= delay;
        }

        public long GetDelay(FunctionName functionName, long groupSettingsId)
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
                    return groupSettings.RetryTimeSendNewFriend;
                }
                case FunctionName.SendMessageToUnanswered:
                {
                    return groupSettings.RetryTimeSendUnanswered;
                }
                case FunctionName.SendMessageToUnread:
                {
                    return groupSettings.RetryTimeSendUnread;
                }
                case FunctionName.RefreshFriends:
                {
                    return groupSettings.RetryTimeRefreshFriends;
                }
                case FunctionName.GetNewFriendsAndRecommended:
                {
                    return groupSettings.RetryTimeGetNewAndRecommendedFriends;
                }
                case FunctionName.ConfirmFriendship:
                {
                    return groupSettings.RetryTimeConfirmFriendships;
                }
                case FunctionName.SendRequestFriendship:
                {
                    return groupSettings.RetryTimeSendRequestFriendships;
                }
                default:
                    throw new ArgumentOutOfRangeException("functionName");
            }
        }
    }
}
