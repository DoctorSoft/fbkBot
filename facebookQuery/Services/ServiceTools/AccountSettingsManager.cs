using CommonModels;
using DataBase.Context;
using DataBase.QueriesAndCommands.Commands.Settings;
using DataBase.QueriesAndCommands.Queries.Settings;
using Services.Core.Interfaces.ServiceTools;

namespace Services.ServiceTools
{
    public class AccountSettingsManager : IAccountSettingsManager
    {
        public SettingsModel GetSettings(long groupSettingsId)
        {
            var settings = new GetSettingsByGroupSettingsIdHandler(new DataBaseContext()).Handle(new GetSettingsByGroupSettingsIdQuery
            {
                GroupSettingsId = groupSettingsId
            });

            if (settings == null)
            {
                return null;
            }

            return new SettingsModel
            {
                GroupId = settings.GroupId,
                Gender = settings.Gender,
                LivesPlace = settings.LivesPlace,
                SchoolPlace = settings.SchoolPlace,
                WorkPlace = settings.WorkPlace,
                DelayTimeSendUnanswered = settings.DelayTimeSendUnanswered,
                DelayTimeSendNewFriend = settings.DelayTimeSendNewFriend,
                DelayTimeSendUnread = settings.DelayTimeSendUnread
            };
        }

        public void UpdateSettings(SettingsModel newSettings)
        {
            new AddOrUpdateSettingsCommandHandler(new DataBaseContext()).Handle(new AddOrUpdateSettingsCommand
                {
                    GroupId = newSettings.GroupId,
                    Gender = newSettings.Gender,
                    LivesPlace = newSettings.LivesPlace,
                    SchoolPlace = newSettings.SchoolPlace,
                    WorkPlace = newSettings.WorkPlace,
                    DelayTimeSendNewFriend = newSettings.DelayTimeSendNewFriend,
                    DelayTimeSendUnanswered = newSettings.DelayTimeSendUnanswered,
                    DelayTimeSendUnread = newSettings.DelayTimeSendUnread
                });
        }
    }
}
