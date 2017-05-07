using System;
using CommonModels;
using Constants.FunctionEnums;
using DataBase.Context;
using DataBase.QueriesAndCommands.Queries.Settings;
using Services.Interfaces.ServiceTools;

namespace Services.ServiceTools
{
    public class SettingsManager : ISettingsManager
    {

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
                case FunctionName.JoinTheNewGroupsAndPages:
                {
                    return new TimeSpan(0, 0, 10);
                }
                case FunctionName.InviteToGroups:
                {
                    var inviteToGroupsTimeModel = groupSettings.CommunityOptions.RetryTimeInviteTheGroups;
                    return new TimeSpan(inviteToGroupsTimeModel.Hours, inviteToGroupsTimeModel.Minutes,
                        inviteToGroupsTimeModel.Seconds);
                }
                case FunctionName.InviteToPages:
                {
                    var inviteToPagesTimeModel = groupSettings.CommunityOptions.RetryTimeInviteThePages;
                    return new TimeSpan(inviteToPagesTimeModel.Hours, inviteToPagesTimeModel.Minutes,
                        inviteToPagesTimeModel.Seconds);
                }
                case FunctionName.Wink:
                {
                    var winkFriendsTimeModel = groupSettings.WinkOptions.RetryTimeForWinkFriends;
                    return new TimeSpan(winkFriendsTimeModel.Hours, winkFriendsTimeModel.Minutes,
                        winkFriendsTimeModel.Seconds);
                }
                case FunctionName.WinkFriendFriends:
                {
                    var winkFriendsFriendsTimeModel = groupSettings.WinkOptions.RetryTimeForWinkFriendsFriends;
                    return new TimeSpan(winkFriendsFriendsTimeModel.Hours, winkFriendsFriendsTimeModel.Minutes,
                        winkFriendsFriendsTimeModel.Seconds);
                }
                case FunctionName.WinkBack:
                {
                    var winkBackTimeModel = groupSettings.WinkOptions.RetryTimeForWinkBack;
                    return new TimeSpan(winkBackTimeModel.Hours, winkBackTimeModel.Minutes,
                        winkBackTimeModel.Seconds);
                }
                default:
                    throw new ArgumentOutOfRangeException("functionName");
            }
        }
    }
}
