using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.Settings
{
    public class GetSettingsByGroupSettingsIdHandler : IQueryHandler<GetSettingsByGroupSettingsIdQuery, SettingsData>
    {
        private readonly DataBaseContext context;

        public GetSettingsByGroupSettingsIdHandler(DataBaseContext context)
        {
            this.context = context;
        }

        public SettingsData Handle(GetSettingsByGroupSettingsIdQuery query)
        {
            var settings =
                context.Settings.Where(model => model.Id == query.GroupSettingsId)
                    .Select(model => new SettingsData
                    {
                        GroupId = model.Id,
                        Gender = model.Gender,
                        LivesPlace = model.LivesPlace,
                        SchoolPlace = model.SchoolPlace,
                        WorkPlace = model.WorkPlace,
                        DelayTimeSendUnread = model.DelayTimeSendUnread,
                        DelayTimeSendNewFriend = model.DelayTimeSendNewFriend,
                        DelayTimeSendUnanswered = model.DelayTimeSendUnanswered
                    }).FirstOrDefault();

            return settings;
        }
    }
}
