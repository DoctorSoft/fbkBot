using System;
using System.Collections.Generic;
using System.Linq;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.Settings;
using DataBase.QueriesAndCommands.Queries.Settings;
using Hangfire;
using Newtonsoft.Json;
using Services.Core.Interfaces.ServiceTools;
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
                Gender = settings.Gender,
                Cities = ConvertJsonToString(settings.Cities),
                Countries = ConvertJsonToString(settings.Countries),
                RetryTimeSendUnread = settings.RetryTimeSendUnread,
                RetryTimeConfirmFriendships = settings.RetryTimeConfirmFriendships,
                RetryTimeGetNewAndRecommendedFriends = settings.RetryTimeGetNewAndRecommendedFriends,
                RetryTimeRefreshFriends = settings.RetryTimeRefreshFriends,
                RetryTimeSendNewFriend = settings.RetryTimeSendNewFriend,
                RetryTimeSendRequestFriendships = settings.RetryTimeSendRequestFriendships,
                RetryTimeSendUnanswered = settings.RetryTimeSendUnanswered,
                UnansweredDelay = settings.UnansweredDelay
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
                    Gender = newSettings.Gender,
                    Cities = newSettings.Cities,
                    Countries = newSettings.Countries,
                    RetryTimeSendUnread = newSettings.RetryTimeSendUnread,
                    RetryTimeConfirmFriendships = newSettings.RetryTimeConfirmFriendships,
                    RetryTimeGetNewAndRecommendedFriends = newSettings.RetryTimeGetNewAndRecommendedFriends,
                    RetryTimeRefreshFriends = newSettings.RetryTimeRefreshFriends,
                    RetryTimeSendNewFriend = newSettings.RetryTimeSendNewFriend,
                    RetryTimeSendRequestFriendships = newSettings.RetryTimeSendRequestFriendships,
                    RetryTimeSendUnanswered = newSettings.RetryTimeSendUnanswered,
                    UnansweredDelay = newSettings.UnansweredDelay
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
