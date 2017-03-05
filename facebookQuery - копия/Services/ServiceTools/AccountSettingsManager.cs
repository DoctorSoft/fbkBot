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
                return null;
            }

            return new GroupSettingsViewModel
            {
                GroupId = settings.GroupId,
                Gender = settings.GeoOptions.Gender,
                Cities = ConvertJsonToString(settings.GeoOptions.Cities),
                Countries = ConvertJsonToString(settings.GeoOptions.Countries),
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
                RetryTimeSendNewFriendHour = settings.MessageOptions.RetryTimeSendNewFriend.Hours,
                RetryTimeSendNewFriendMin = settings.MessageOptions.RetryTimeSendNewFriend.Minutes,
                RetryTimeSendNewFriendSec = settings.MessageOptions.RetryTimeSendNewFriend.Seconds,
                RetryTimeSendRequestFriendshipsHour = settings.FriendsOptions.RetryTimeSendRequestFriendships.Hours,
                RetryTimeSendRequestFriendshipsMin = settings.FriendsOptions.RetryTimeSendRequestFriendships.Minutes,
                RetryTimeSendRequestFriendshipsSec = settings.FriendsOptions.RetryTimeSendRequestFriendships.Seconds,
                RetryTimeSendUnansweredHour = settings.MessageOptions.RetryTimeSendUnanswered.Hours,
                RetryTimeSendUnansweredMin = settings.MessageOptions.RetryTimeSendUnanswered.Minutes,
                RetryTimeSendUnansweredSec = settings.MessageOptions.RetryTimeSendUnanswered.Seconds,
                UnansweredDelay = settings.MessageOptions.UnansweredDelay
            };
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
