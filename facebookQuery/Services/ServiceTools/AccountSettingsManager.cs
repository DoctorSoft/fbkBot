using System;
using System.Collections.Generic;
using System.Linq;
using CommonModels;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.Settings;
using DataBase.QueriesAndCommands.Models.JsonModels;
using DataBase.QueriesAndCommands.Queries.Settings;
using Hangfire;
using Newtonsoft.Json;
using Services.Interfaces.ServiceTools;
using Services.ViewModels.GroupModels;

namespace Services.ServiceTools
{
    public class AccountSettingsManager : IAccountSettingsManager
    {
        public GroupSettingsViewModel GetSettings(long groupSettingsId)
        {
            var settings = new GetSettingsByGroupSettingsIdQueryHandler(new DataBaseContext()).Handle(new GetSettingsByGroupSettingsIdQuery
            {
                GroupSettingsId = groupSettingsId
            });

            if (settings == null)
            {
                return new GroupSettingsViewModel();
            }

            return new GroupSettingsViewModel
            {
                GroupId = settings.GroupId,
                IsJoinToAllGroups = settings.CommunityOptions.IsJoinToAllGroups,
                RetryTimeInviteTheGroupsHour = settings.CommunityOptions.RetryTimeInviteTheGroups == null ? 0 : settings.CommunityOptions.RetryTimeInviteTheGroups.Hours,
                RetryTimeInviteTheGroupsMin = settings.CommunityOptions.RetryTimeInviteTheGroups == null ? 0 : settings.CommunityOptions.RetryTimeInviteTheGroups.Minutes,
                RetryTimeInviteTheGroupsSec = settings.CommunityOptions.RetryTimeInviteTheGroups == null ? 0 : settings.CommunityOptions.RetryTimeInviteTheGroups.Seconds,
                RetryTimeInviteThePagesHour = settings.CommunityOptions.RetryTimeInviteThePages == null ? 0 : settings.CommunityOptions.RetryTimeInviteThePages.Hours,
                RetryTimeInviteThePagesMin = settings.CommunityOptions.RetryTimeInviteThePages == null ? 0 : settings.CommunityOptions.RetryTimeInviteThePages.Minutes,
                RetryTimeInviteThePagesSec = settings.CommunityOptions.RetryTimeInviteThePages == null ? 0 : settings.CommunityOptions.RetryTimeInviteThePages.Seconds,
                FacebookGroups = ConvertListToString(settings.CommunityOptions.Groups),
                FacebookPages = ConvertListToString(settings.CommunityOptions.Pages),
                MaxFriendsJoinGroupInDay = settings.CommunityOptions.MaxFriendsJoinGroupInDay,
                MinFriendsJoinGroupInDay = settings.CommunityOptions.MinFriendsJoinGroupInDay,
                MaxFriendsJoinGroupInHour = settings.CommunityOptions.MaxFriendsJoinGroupInHour,
                MinFriendsJoinGroupInHour = settings.CommunityOptions.MinFriendsJoinGroupInHour,
                MaxFriendsJoinPageInDay = settings.CommunityOptions.MaxFriendsJoinPageInDay,
                MinFriendsJoinPageInDay = settings.CommunityOptions.MinFriendsJoinPageInDay,
                MaxFriendsJoinPageInHour = settings.CommunityOptions.MaxFriendsJoinPageInHour,
                MinFriendsJoinPageInHour = settings.CommunityOptions.MinFriendsJoinPageInHour,
                Gender = settings.GeoOptions.Gender,
                Cities = settings.GeoOptions.Cities,
                Countries = settings.GeoOptions.Countries,

                //friends options
                RetryTimeSendUnreadHour = settings.MessageOptions.RetryTimeSendUnread.Hours,
                RetryTimeSendUnreadMin = settings.MessageOptions.RetryTimeSendUnread.Minutes,
                RetryTimeSendUnreadSec = settings.MessageOptions.RetryTimeSendUnread.Seconds,
                RetryTimeConfirmFriendshipsHour = settings.FriendsOptions.RetryTimeConfirmFriendships.Hours,
                RetryTimeConfirmFriendshipsMin = settings.FriendsOptions.RetryTimeConfirmFriendships.Minutes,
                RetryTimeConfirmFriendshipsSec = settings.FriendsOptions.RetryTimeConfirmFriendships.Seconds,
                RetryTimeGetNewAndRecommendedFriendsHour = settings.FriendsOptions.RetryTimeGetNewAndRecommendedFriends.Hours,
                RetryTimeGetNewAndRecommendedFriendsMin = settings.FriendsOptions.RetryTimeGetNewAndRecommendedFriends.Minutes,
                RetryTimeGetNewAndRecommendedFriendsSec = settings.FriendsOptions.RetryTimeGetNewAndRecommendedFriends.Seconds,
                RetryTimeRefreshFriendsHour = settings.FriendsOptions.RetryTimeRefreshFriends.Hours,
                RetryTimeRefreshFriendsMin = settings.FriendsOptions.RetryTimeRefreshFriends.Minutes,
                RetryTimeRefreshFriendsSec = settings.FriendsOptions.RetryTimeRefreshFriends.Seconds,
                AllowedRemovalPercentage = settings.FriendsOptions.AllowedRemovalPercentage,

                //messages options
                GetMessagesFromThoseConnectedToMessenger = settings.MessageOptions.GetMessagesFromThoseConnectedToMessenger,
                RetryTimeSendNewFriendHour = settings.MessageOptions.RetryTimeSendNewFriend.Hours,
                RetryTimeSendNewFriendMin = settings.MessageOptions.RetryTimeSendNewFriend.Minutes,
                RetryTimeSendNewFriendSec = settings.MessageOptions.RetryTimeSendNewFriend.Seconds,
                RetryTimeSendRequestFriendshipsHour = settings.FriendsOptions.RetryTimeSendRequestFriendships.Hours,
                RetryTimeSendRequestFriendshipsMin = settings.FriendsOptions.RetryTimeSendRequestFriendships.Minutes,
                RetryTimeSendRequestFriendshipsSec = settings.FriendsOptions.RetryTimeSendRequestFriendships.Seconds,
                RetryTimeSendUnansweredHour = settings.MessageOptions.RetryTimeSendUnanswered.Hours,
                RetryTimeSendUnansweredMin = settings.MessageOptions.RetryTimeSendUnanswered.Minutes,
                RetryTimeSendUnansweredSec = settings.MessageOptions.RetryTimeSendUnanswered.Seconds,
                UnansweredDelay = settings.MessageOptions.UnansweredDelay,

                //limits options
                CountMaxFriends = settings.LimitsOptions.CountMaxFriends,
                CountMinFriends = settings.LimitsOptions.CountMinFriends,

                //delete friends settings 
                DialogIsOverTimer = settings.DeleteFriendsOptions.DialogIsOver == null ? 0 : settings.DeleteFriendsOptions.DialogIsOver.Timer,
                EnableDialogIsOver = settings.DeleteFriendsOptions.DialogIsOver != null && settings.DeleteFriendsOptions.DialogIsOver.IsEnabled,
                IsAddedToGroupsAndPagesTimer = settings.DeleteFriendsOptions.IsAddedToGroupsAndPages == null ? 0 : settings.DeleteFriendsOptions.IsAddedToGroupsAndPages.Timer,
                EnableIsAddedToGroupsAndPages = settings.DeleteFriendsOptions.IsAddedToGroupsAndPages != null && settings.DeleteFriendsOptions.IsAddedToGroupsAndPages.IsEnabled,
                IsWinkTimer = settings.DeleteFriendsOptions.IsWink == null ? 0 : settings.DeleteFriendsOptions.IsWink.Timer,
                EnableIsWink = settings.DeleteFriendsOptions.IsWink != null && settings.DeleteFriendsOptions.IsWink.IsEnabled,
                IsWinkFriendsOfFriendsTimer = settings.DeleteFriendsOptions.IsWinkFriendsOfFriends == null ? 0 : settings.DeleteFriendsOptions.IsWinkFriendsOfFriends.Timer,
                EnableIsWinkFriendsOfFriends = settings.DeleteFriendsOptions.IsWinkFriendsOfFriends != null && settings.DeleteFriendsOptions.IsWinkFriendsOfFriends.IsEnabled,
                DeletionFriendTimer = settings.DeleteFriendsOptions.DeletionFriendTimer,

                //winks
                RetryTimeForWinkFriendsHour = settings.WinkOptions.RetryTimeForWinkFriends == null ? 0 : settings.WinkOptions.RetryTimeForWinkFriends.Hours,
                RetryTimeForWinkFriendsMin = settings.WinkOptions.RetryTimeForWinkFriends == null ? 0 : settings.WinkOptions.RetryTimeForWinkFriends.Minutes,
                RetryTimeForWinkFriendsSec = settings.WinkOptions.RetryTimeForWinkFriends == null ? 0 : settings.WinkOptions.RetryTimeForWinkFriends.Seconds,
                ConsiderGeoForWinkFriends = settings.WinkOptions.ConsiderGeoForWinkFriends,
                RetryTimeForWinkFriendsFriendsHour = settings.WinkOptions.RetryTimeForWinkFriendsFriends == null ? 0 : settings.WinkOptions.RetryTimeForWinkFriendsFriends.Hours,
                RetryTimeForWinkFriendsFriendsMin = settings.WinkOptions.RetryTimeForWinkFriendsFriends == null ? 0 : settings.WinkOptions.RetryTimeForWinkFriendsFriends.Minutes,
                RetryTimeForWinkFriendsFriendsSec = settings.WinkOptions.RetryTimeForWinkFriendsFriends == null ? 0 : settings.WinkOptions.RetryTimeForWinkFriendsFriends.Seconds,
                ConsiderGeoForWinkFriendsFriends = settings.WinkOptions.ConsiderGeoForWinkFriendsFriends,
                RetryTimeForWinkBackHour = settings.WinkOptions.RetryTimeForWinkBack == null ? 0 : settings.WinkOptions.RetryTimeForWinkBack.Hours,
                RetryTimeForWinkBackMin = settings.WinkOptions.RetryTimeForWinkBack == null ? 0 : settings.WinkOptions.RetryTimeForWinkBack.Minutes,
                RetryTimeForWinkBackSec = settings.WinkOptions.RetryTimeForWinkBack == null ? 0 : settings.WinkOptions.RetryTimeForWinkBack.Seconds
            };
        }

        private static string ConvertListToString(IEnumerable<string> arrayString)
        {
            if (arrayString == null)
            {
                return string.Empty;
            }

            return arrayString.Aggregate("", (current, element) => current + (element + "\n"));
        }

        public string GetCronByMinutes(long min)
        {
            var minutesTimeSpan = TimeSpan.FromMinutes(min);

            if (min <= 0)
            {
                return Cron.Hourly();
            }

            if (minutesTimeSpan.Hours == 0)
            {
                var result = string.Format("0/{0} * * * *", minutesTimeSpan.Minutes);

                return result;
            }

            if (minutesTimeSpan.Hours != 0)
            {
                var result = string.Format("{0} {1} * * *",
                minutesTimeSpan.Minutes != 0 ? string.Format("{0}", minutesTimeSpan.Minutes) : "0",
                minutesTimeSpan.Hours != 0 ? string.Format("0/{0}", minutesTimeSpan.Hours) : "*");

                return result;
            }

            return Cron.Hourly(); 
        }

        public void UpdateSettings(GroupSettingsViewModel newSettings)
        {
            new AddOrUpdateSettingsCommandHandler(new DataBaseContext()).Handle(new AddOrUpdateSettingsCommand
            {
                GroupId = newSettings.GroupId,
                GeoOptions = new GeoOptionsDbModel
                {
                    Cities = newSettings.Cities,
                    Countries = newSettings.Countries,
                    Gender = newSettings.Gender
                },
                FriendsOptions = new FriendOptionsDbModel
                {
                    RetryTimeConfirmFriendships = new TimeModel
                    {
                        Hours = newSettings.RetryTimeConfirmFriendshipsHour,
                        Minutes = newSettings.RetryTimeConfirmFriendshipsMin,
                        Seconds = newSettings.RetryTimeConfirmFriendshipsSec
                    },
                    RetryTimeGetNewAndRecommendedFriends = new TimeModel
                    {
                        Hours = newSettings.RetryTimeGetNewAndRecommendedFriendsHour,
                        Minutes = newSettings.RetryTimeGetNewAndRecommendedFriendsMin,
                        Seconds = newSettings.RetryTimeGetNewAndRecommendedFriendsSec
                    },
                    RetryTimeRefreshFriends = new TimeModel
                    {
                        Hours = newSettings.RetryTimeRefreshFriendsHour,
                        Minutes = newSettings.RetryTimeRefreshFriendsMin,
                        Seconds = newSettings.RetryTimeRefreshFriendsSec
                    },
                    RetryTimeSendRequestFriendships = new TimeModel
                    {
                        Hours = newSettings.RetryTimeSendRequestFriendshipsHour,
                        Minutes = newSettings.RetryTimeSendRequestFriendshipsMin,
                        Seconds = newSettings.RetryTimeSendRequestFriendshipsSec
                    },
                },
                MessageOptions = new MessageOptionsDbModel
                {
                    GetMessagesFromThoseConnectedToMessenger = newSettings.GetMessagesFromThoseConnectedToMessenger,
                    RetryTimeSendNewFriend =
                    {
                        Hours = newSettings.RetryTimeSendUnreadHour,
                        Minutes = newSettings.RetryTimeSendUnreadMin,
                        Seconds = newSettings.RetryTimeSendUnreadSec
                    },
                    RetryTimeSendUnanswered =
                    {
                        Hours = newSettings.RetryTimeSendUnansweredHour,
                        Minutes = newSettings.RetryTimeSendUnansweredMin,
                        Seconds = newSettings.RetryTimeSendUnansweredSec
                    },
                    RetryTimeSendUnread =
                    {
                        Hours = newSettings.RetryTimeSendUnreadHour,
                        Minutes = newSettings.RetryTimeSendUnreadMin,
                        Seconds = newSettings.RetryTimeSendUnreadSec
                    },
                    UnansweredDelay = newSettings.UnansweredDelay
                }
            });
        }
        
        private string ConvertJsonToString(string jsonData)
        {
            try
            {
                var words = JsonConvert.DeserializeObject<List<string>>(jsonData);

                var result = string.Join("\r\n", words.Select(s => s));

                return result;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
