using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Queries.Settings
{
    public class GetSettingsByGroupSettingsIdQueryHandler : IQueryHandler<GetSettingsByGroupSettingsIdQuery, SettingsData>
    {
        private readonly DataBaseContext context;

        public GetSettingsByGroupSettingsIdQueryHandler(DataBaseContext context)
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
                        Countries = model.Countries,
                        Cities = model.Cities,
                        RetryTimeSendUnread = model.RetryTimeSendUnread,
                        RetryTimeConfirmFriendships = model.RetryTimeConfirmFriendships,
                        RetryTimeGetNewAndRecommendedFriends = model.RetryTimeGetNewAndRecommendedFriends,
                        RetryTimeRefreshFriends = model.RetryTimeRefreshFriends,
                        RetryTimeSendNewFriend = model.RetryTimeSendNewFriend,
                        RetryTimeSendRequestFriendships = model.RetryTimeSendRequestFriendships,
                        RetryTimeSendUnanswered = model.RetryTimeSendUnanswered,
                        UnansweredDelay = model.UnansweredDelay
                    }).FirstOrDefault();

            return settings;
        }
    }
}
