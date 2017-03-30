using System;
using System.Linq;
using System.Web.Script.Serialization;
using DataBase.Context;
using DataBase.QueriesAndCommands.Models.JsonModels;

namespace DataBase.QueriesAndCommands.Queries.Settings
{
    public class GetSettingsByGroupSettingsIdQueryHandler :
        IQueryHandler<GetSettingsByGroupSettingsIdQuery, SettingsData>
    {
        private readonly DataBaseContext _context;

        public GetSettingsByGroupSettingsIdQueryHandler(DataBaseContext context)
        {
            this._context = context;
        }

        public SettingsData Handle(GetSettingsByGroupSettingsIdQuery query)
        {
            var settings =
                _context.Settings.FirstOrDefault(model => model.Id == query.GroupSettingsId);

            if (settings == null)
            {
                return null;
            }
            try
            {
                var jsDeserializator = new JavaScriptSerializer();

                var friendOptionsModel = jsDeserializator.Deserialize<FriendOptionsDbModel>(settings.FriendsOptions);
                var geoOptionsModel = jsDeserializator.Deserialize<GeoOptionsDbModel>(settings.GeoOptions);
                var messageOptionsModel = jsDeserializator.Deserialize<MessageOptionsDbModel>(settings.MessageOptions);
                var limitsOptionsModel = jsDeserializator.Deserialize<LimitsOptionsDbModel>(settings.LimitsOptions);
                var communityOptionsModel = jsDeserializator.Deserialize<CommunityOptionsDbModel>(settings.CommunityOptions);
                var deleteFriendsOptionsModel = jsDeserializator.Deserialize<DeleteFriendsOptionsDbModel>(settings.DeleteFriendsOptions);
                var winkFriendsOptionsModel = jsDeserializator.Deserialize<WinkFriendsOptionsDbModel>(settings.WinkFriendsOptions);

                return new SettingsData
                {
                    Id = settings.Id,
                    GroupId = query.GroupSettingsId,
                    FriendsOptions = friendOptionsModel,
                    GeoOptions = geoOptionsModel,
                    MessageOptions = messageOptionsModel,
                    LimitsOptions = limitsOptionsModel,
                    CommunityOptions = communityOptionsModel,
                    DeleteFriendsOptions = deleteFriendsOptionsModel,
                    WinkOptions = winkFriendsOptionsModel
                };
            }
            catch (Exception ex)
            {

            }

            return null;
        }
    }
}

